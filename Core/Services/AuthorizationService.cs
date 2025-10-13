using Core.Abstractions;
using Core.Models;
using Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;
public sealed class AuthorizationService
{
    private readonly IClientStore _clients;
    private readonly IAuthorizationCodeStore _codes;
    private readonly ISubjectResolver _subject;
    private readonly IOptions<OidcServerOptions> _opt;

    public AuthorizationService(IClientStore clients, IAuthorizationCodeStore codes, ISubjectResolver subject, IOptions<OidcServerOptions> opt)
    { _clients = clients; _codes = codes; _subject = subject; _opt = opt; }

    public async Task<AuthorizeOutcome> AuthorizeAsync(AuthorizeRequest req, ClaimsPrincipal user, string? sessionId, CancellationToken ct = default)
    {
        var sub = _subject.ResolveSubject(user);
        if (string.IsNullOrEmpty(sub))
            return new ErrorOutcome("login_required");

        if (req.ResponseType != "code")
            return new ErrorOutcome("unsupported_response_type");

        var client = await _clients.FindByIdAsync(req.ClientId, ct);
        if (client is null) return new ErrorOutcome("invalid_client");
        if (!await _clients.IsRedirectUriValidAsync(client, req.RedirectUri, ct)) return new ErrorOutcome("invalid_request", "redirect_uri");
        var scopes = (req.Scope ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (!await _clients.IsScopeAllowedAsync(client, scopes, ct)) return new ErrorOutcome("invalid_scope");
        if (client.RequirePkce && (string.IsNullOrEmpty(req.CodeChallenge) || req.CodeChallengeMethod != "S256"))
            return new ErrorOutcome("invalid_request", "PKCE required");

        // 認可コード生成（保存はハッシュ）
        var code = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        var hash = HexSha256(code);
        await _codes.StoreAsync(new AuthorizationCodeRecord
        {
            CodeHash = hash,
            ClientId = req.ClientId,
            Subject = sub!,
            RedirectUri = req.RedirectUri,
            Scope = req.Scope,
            ExpiresAt = DateTimeOffset.UtcNow.Add(_opt.Value.AuthorizationCodeLifetime),
            CodeChallenge = req.CodeChallenge,
            CodeChallengeMethod = req.CodeChallengeMethod,
            Nonce = req.Nonce,
            SessionId = sessionId
        }, ct);

        var qs = $"code={Uri.EscapeDataString(code)}&state={Uri.EscapeDataString(req.State ?? "")}";
        var redirect = req.RedirectUri.Contains('?') ? $"{req.RedirectUri}&{qs}" : $"{req.RedirectUri}?{qs}";
        return new RedirectOutcome(redirect);

        static string HexSha256(string s)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.ASCII.GetBytes(s)));
        }
    }
}