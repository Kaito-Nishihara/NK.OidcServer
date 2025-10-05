using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Events
{
    public sealed class UserRegisteredEvent : DomainEventBase
    {
        public Guid UserId { get; }
        public string Email { get; }

        public UserRegisteredEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
