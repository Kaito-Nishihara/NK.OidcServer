using Core.Abstractions;
using Core.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;
public sealed class DiscoveryService
{
    private readonly IOptions<OidcServerOptions> _opt;
    private readonly IJwtIssuerBridge _jwt;

    public DiscoveryService(IOptions<OidcServerOptions> opt, IJwtIssuerBridge jwt)
    { _opt = opt; _jwt = jwt; }

    public object BuildConfiguration() => new
    {
        issuer = _opt.Value.Issuer,
        authorization_endpoint = "/connect/authorize",
        token_endpoint = "/connect/token",
        userinfo_endpoint = "/connect/userinfo",
        jwks_uri = "/.well-known/jwks.json",
        response_types_supported = new[] { "code" },
        grant_types_supported = new[] { "authorization_code", "refresh_token" },
        code_challenge_methods_supported = new[] { "S256" },
        scopes_supported = new[] { "openid", "profile", "email", "offline_access" },
        id_token_signing_alg_values_supported = new[] { "RS256" }
    };

    public object BuildJwks() => _jwt.ExportJwks();
}