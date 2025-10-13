using Core.Abstractions;
using System.Collections.Concurrent;

namespace Infrastructure.Stores;
public sealed class InMemoryAuthorizationCodeStore : IAuthorizationCodeStore
{
    private readonly ConcurrentDictionary<string, AuthorizationCodeRecord> _codes = new();
    public Task StoreAsync(AuthorizationCodeRecord rec, CancellationToken ct = default)
    { _codes[rec.CodeHash] = rec; return Task.CompletedTask; }
    public Task<AuthorizationCodeRecord?> FindAsync(string codeHash, CancellationToken ct = default)
    { _codes.TryGetValue(codeHash, out var rec); return Task.FromResult(rec); }
    public Task ConsumeAsync(string codeHash, CancellationToken ct = default)
    { _codes.TryRemove(codeHash, out _); return Task.CompletedTask; }
}

