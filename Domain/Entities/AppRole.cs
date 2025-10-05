using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppRole : AuditableAggregate
    {
        public string Name { get; private set; } = default!;
        public ICollection<AppUserRole> UserRoles { get; private set; } = new List<AppUserRole>();

        private AppRole() { }

        public AppRole(string name)
        {
            Name = name;
        }
    }
}
