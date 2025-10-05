using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppUserRole
    {
        public Guid UserId { get; private set; }
        public AppUser User { get; private set; } = default!;

        public Guid RoleId { get; private set; }
        public AppRole Role { get; private set; } = default!;

        private AppUserRole() { }

        public AppUserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }

}
