using Domain.Common;
using Domain.Common.Events;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identites.Core.Abstractions;
namespace Domain.Entities
{
    public class AppUser : IAuthUser<Guid>
    {
        public Guid Id => throw new NotImplementedException();
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NormalizedUserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NormalizedEmail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EmailConfirmed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? PhoneNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool PhoneNumberConfirmed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PasswordHash { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SecurityStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ConcurrencyStamp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int AccessFailedCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool LockoutEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? LockoutEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool TwoFactorEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
