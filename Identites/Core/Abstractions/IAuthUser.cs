using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public interface IAuthUser<TKey>
    {
        TKey Id { get; }

        string UserName { get; set; }
        string NormalizedUserName { get; set; }

        string? Email { get; set; }
        string NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }

        string? PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }

        string PasswordHash { get; set; }
        string SecurityStamp { get; set; }
        string ConcurrencyStamp { get; set; }

        int AccessFailedCount { get; set; }
        bool LockoutEnabled { get; set; }
        DateTimeOffset? LockoutEnd { get; set; }

        bool TwoFactorEnabled { get; set; }
    }
}
