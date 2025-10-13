using Core.Abstractions;
using Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;
public sealed class RsaJwtIssuerBridge : IJwtIssuerBridge
{
    private readonly RSA _rsa;
    private readonly string _kid;
    private readonly OidcServerOptions _opt;

    public RsaJwtIssuerBridge(IOptions<OidcServerOptions> opt)
    {
        _opt = opt.Value;
        _rsa = RSA.Create(2048);
        _kid = "kid-1"; // 固定でもOK。運用でローテーションするなら別途管理
    }

    public TokenPair IssueTokens(string subject, IEnumerable<string> scopes, string clientId, string? nonce)
    {
        var now = DateTimeOffset.UtcNow;
        var expires = now.Add(_opt.AccessTokenLifetime);

        var creds = new SigningCredentials(new RsaSecurityKey(_rsa) { KeyId = _kid }, SecurityAlgorithms.RsaSha256);
        var claims = new List<Claim> {
            new("sub", subject),
            new("client_id", clientId),
            new("scope", string.Join(' ', scopes)),
        };
        var at = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.DefaultAudience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: expires.UtcDateTime,
            signingCredentials: creds);

        // 必要に応じて id_token を発行（nonce が来た場合のみなど）
        string? idToken = null;
        if (!string.IsNullOrEmpty(nonce))
        {
            var idClaims = new List<Claim> {
                new("sub", subject),
                new("aud", clientId),
                new("nonce", nonce!)
            };
            var id = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: clientId,
                claims: idClaims,
                notBefore: now.UtcDateTime,
                expires: now.AddMinutes(5).UtcDateTime,
                signingCredentials: creds);
            idToken = new JwtSecurityTokenHandler().WriteToken(id);
        }

        var accessToken = new JwtSecurityTokenHandler().WriteToken(at);

        // refresh_token はここでは未実装（必要なら別ストアで発行・ローテーション）
        return new TokenPair(accessToken, expires, idToken, RefreshToken: null);
    }

    public object ExportJwks()
    {
        var p = _rsa.ExportParameters(false);
        static string B64Url(byte[] x) =>
            Convert.ToBase64String(x).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return new
        {
            keys = new[] {
                new { kty="RSA", use="sig", alg="RS256", kid=_kid, n=B64Url(p.Modulus!), e=B64Url(p.Exponent!) }
            }
        };
    }

    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        throw new NotImplementedException();
    }
}