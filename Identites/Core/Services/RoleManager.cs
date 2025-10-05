using Identites.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Services
{

    public sealed class RoleManager<TRole, TKey>
        where TRole : class, IAuthRole<TKey>
    {
        private readonly IRoleStore<TRole, TKey> _roles;
        private readonly ILookupNormalizer _norm;

        public RoleManager(IRoleStore<TRole, TKey> roles, ILookupNormalizer norm)
        { _roles = roles; _norm = norm; }

        public async Task<Result<TRole>> CreateAsync(TRole role, CancellationToken ct = default)
        {
            role.NormalizedName = _norm.Normalize(role.Name);
            if (await _roles.FindByNameAsync(role.NormalizedName, ct) is not null)
                return Result<TRole>.Fail("Role already exists.");
            await _roles.CreateAsync(role, ct);
            return Result<TRole>.Success(role);
        }
    }
}
