using Core.Abstractions;
using Core.Models;
using Core.Options;
using Core.Util;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Core.Services;
public sealed class TokenService
{
    private readonly IAuthorizationCodeStore _codes;
    private readonly IJwtIssuerBridge _jwt;
    private readonly IOptions<OidcServerOptions> _opt;

    public TokenService(IAuthorizationCodeStore codes, IJwtIssuerBridge jwt, IOptions<OidcServerOptions> opt)
    { _codes = codes; _jwt = jwt; _opt = opt; }

    public async Task<TokenOutcome> HandleAuthorizationCodeAsync(AuthCodeGrant grant, CancellationToken ct = default)
    {
        var codeHash = HexSha256(grant.Code);
        var rec = await _codes.FindAsync(codeHash, ct);
        if (rec is null || rec.ExpiresAt < DateTimeOffset.UtcNow) return new TokenError("invalid_grant");
        if (!string.Equals(rec.RedirectUri, grant.RedirectUri, StringComparison.Ordinal)) return new TokenError("invalid_grant");

        // PKCE 検証
        if (rec.CodeChallengeMethod != "S256" || Pkce.HashS256(grant.CodeVerifier) != rec.CodeChallenge)
            return new TokenError("invalid_grant");

        await _codes.ConsumeAsync(codeHash, ct);

        var scopes = rec.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var pair = _jwt.IssueTokens(rec.Subject, scopes, rec.ClientId, rec.Nonce);
        var expires = (int)(pair.AccessTokenExpires - DateTimeOffset.UtcNow).TotalSeconds;
        return new TokenSuccess(pair.AccessToken, expires, pair.IdToken, pair.RefreshToken);

        static string HexSha256(string s) { using var sha = SHA256.Create(); return Convert.ToHexString(sha.ComputeHash(Encoding.ASCII.GetBytes(s))); }
    }
}