using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions;
public sealed class OidcClient
{
    public required string ClientId { get; init; }
    public string? ClientSecret { get; init; }
    public required HashSet<string> RedirectUris { get; init; }
    public bool RequirePkce { get; init; } = true;
    public bool AllowOfflineAccess { get; init; } = true;
    public HashSet<string> AllowedScopes { get; init; } = new(StringComparer.Ordinal);
}
public interface IClientStore
{
    Task<OidcClient?> FindByIdAsync(string clientId, CancellationToken ct = default);
    Task<bool> IsRedirectUriValidAsync(OidcClient client, string redirectUri, CancellationToken ct = default);
    Task<bool> IsScopeAllowedAsync(OidcClient client, IEnumerable<string> scopes, CancellationToken ct = default);
}
