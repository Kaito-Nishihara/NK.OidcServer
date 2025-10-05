using Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class AggregateRoot<TId> : Entity<TId>
    {
        // ドメインイベントを集約ルート単位で保持できるようにすることが多い
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}
