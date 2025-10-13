using Core.Abstractions;
using System.Collections.Concurrent;

namespace Infrastructure.Stores;
public sealed class InMemoryClientStore : IClientStore
{
    private readonly ConcurrentDictionary<string, OidcClient> _clients;
    public InMemoryClientStore(IEnumerable<OidcClient> clients)
        => _clients = new(clients.Select(c => KeyValuePair.Create(c.ClientId, c)));
    public Task<OidcClient?> FindByIdAsync(string clientId, CancellationToken ct = default)
        => Task.FromResult(_clients.TryGetValue(clientId, out var c) ? c : null);
    public Task<bool> IsRedirectUriValidAsync(OidcClient c, string uri, CancellationToken ct = default)
        => Task.FromResult(c.RedirectUris.Contains(uri, StringComparer.Ordinal));
    public Task<bool> IsScopeAllowedAsync(OidcClient c, IEnumerable<string> scopes, CancellationToken ct = default)
        => Task.FromResult(scopes.All(s => c.AllowedScopes.Contains(s)));
}