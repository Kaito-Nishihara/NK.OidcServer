using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Options;
public sealed class OidcServerOptions
{
    public required string Issuer { get; set; }
    public required string DefaultAudience { get; set; }
    public TimeSpan AuthorizationCodeLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan AccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(30);
    public TimeSpan RefreshTokenLifetime { get; set; } = TimeSpan.FromDays(14);
}
