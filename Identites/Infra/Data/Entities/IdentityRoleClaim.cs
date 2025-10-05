using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Entities;

public class IdentityRoleClaim<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual int Id { get; set; }
    public virtual TKey RoleId { get; set; } = default!;
    public virtual IdentityRole<TKey> Role { get; set; } = default!;
    public virtual string Type { get; set; } = default!;
    public virtual string Value { get; set; } = default!;
}
