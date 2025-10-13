using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions;

public sealed class AuthorizationCodeRecord
{
    public required string CodeHash { get; init; }
    public required string ClientId { get; init; }
    public required string Subject { get; init; }
    public required string RedirectUri { get; init; }
    public required string Scope { get; init; }
    public required DateTimeOffset ExpiresAt { get; init; }
    public required string CodeChallenge { get; init; }
    public required string CodeChallengeMethod { get; init; }
    public string? Nonce { get; init; }
    public string? SessionId { get; init; }
}
public interface IAuthorizationCodeStore
{
    Task StoreAsync(AuthorizationCodeRecord rec, CancellationToken ct = default);
    Task<AuthorizationCodeRecord?> FindAsync(string codeHash, CancellationToken ct = default);
    Task ConsumeAsync(string codeHash, CancellationToken ct = default);
}