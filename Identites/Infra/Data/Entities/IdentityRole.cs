using Identites.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Data.Entities;

/// <summary>
/// 汎用ロール基底クラス（アプリ側で継承して拡張）
/// </summary>
public class IdentityRole<TKey> : IAuthRole<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; } = default!;
    public virtual string? Name { get; set; }
    public virtual string? NormalizedName { get; set; }
    public virtual string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString("N");

    public virtual ICollection<IdentityUserRole<TKey>> UserRoles { get; set; } = new List<IdentityUserRole<TKey>>();
    public virtual ICollection<IdentityRoleClaim<TKey>> Claims { get; set; } = new List<IdentityRoleClaim<TKey>>();
}
