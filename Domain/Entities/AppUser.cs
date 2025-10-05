using Domain.Common;
using Domain.Common.Events;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUser : AuditableAggregate
    {
        public string UserName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string PasswordHash { get; private set; } = default!;
        public bool EmailConfirmed { get; private set; }

        public ICollection<AppUserRole> UserRoles { get; private set; } = new List<AppUserRole>();

        private AppUser() { } // EF用

        private AppUser(string userName, Email email, string passwordHash)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            EmailConfirmed = false;
        }

        /// <summary>
        /// ファクトリメソッド
        /// </summary>
        public static AppUser Create(string userName, Email email, string passwordHash)
        {
            var user = new AppUser(userName, email, passwordHash);

            // ドメインイベント発行
            user.AddDomainEvent(new UserRegisteredEvent(user.Id, user.Email.Value));

            return user;
        }

        public void ConfirmEmail()
        {
            EmailConfirmed = true;
            Touch();
        }
    }
}
