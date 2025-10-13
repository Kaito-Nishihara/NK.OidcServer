using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions;

public sealed record TokenPair(string AccessToken, DateTimeOffset AccessTokenExpires, string? IdToken, string? RefreshToken);
public interface IJwtIssuerBridge
{
    TokenPair IssueTokens(string subject, IEnumerable<string> scopes, string clientId, string? nonce);
    ClaimsPrincipal? ValidateAccessToken(string token);
    object ExportJwks();
}