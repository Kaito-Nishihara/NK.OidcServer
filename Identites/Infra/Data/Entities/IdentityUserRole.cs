using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Entities;

public class IdentityUserRole<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual TKey UserId { get; set; } = default!;
    public virtual IdentityUser<TKey> User { get; set; } = default!;

    public virtual TKey RoleId { get; set; } = default!;
    public virtual IdentityRole<TKey> Role { get; set; } = default!;
}
