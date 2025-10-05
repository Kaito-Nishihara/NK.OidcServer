using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Events
{
    /// <summary>
    /// ドメインイベントの基底クラス
    /// </summary>
    public abstract class DomainEventBase : IDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;

        protected DomainEventBase()
        {
            OccurredOn = DateTime.UtcNow;
        }
    }
}
