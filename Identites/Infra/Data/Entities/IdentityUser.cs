using Identites.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Infra.Data.Entities;

/// <summary>
/// 汎用ユーザー基底クラス（アプリ側で継承して拡張）
/// </summary>
public class IdentityUser<TKey> : IAuthUser<TKey>
    where TKey : IEquatable<TKey>
{
    public virtual TKey Id { get; set; } = default!;

    public virtual string UserName { get; set; }
    public virtual string NormalizedUserName { get; set; }

    public virtual string Email { get; set; }
    public virtual string NormalizedEmail { get; set; }
    public virtual bool EmailConfirmed { get; set; }

    public virtual string PhoneNumber { get; set; }
    public virtual bool PhoneNumberConfirmed { get; set; }

    public virtual string PasswordHash { get; set; }
    public virtual string SecurityStamp { get; set; }
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString("N");

    public virtual bool TwoFactorEnabled { get; set; }

    public virtual int AccessFailedCount { get; set; }
    public virtual bool LockoutEnabled { get; set; } = true;
    public virtual DateTimeOffset? LockoutEnd { get; set; }


    // ナビゲーション
    public virtual ICollection<IdentityUserRole<TKey>> UserRoles { get; set; } = new List<IdentityUserRole<TKey>>();
    public virtual ICollection<IdentityUserClaim<TKey>> Claims { get; set; } = new List<IdentityUserClaim<TKey>>();
}