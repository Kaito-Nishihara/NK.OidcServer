using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public abstract class AuditableAggregate : AggregateRoot<Guid>, IAuditable, ISoftDelete, IHasRowVersion
    {
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;
        public bool IsDeleted { get; protected set; }

        /// <summary>
        /// 楽観的同時実行制御用
        /// </summary>
        public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

        public virtual void Touch() => UpdatedAt = DateTime.UtcNow;

        public virtual void SoftDelete()
        {
            IsDeleted = true;
            Touch();
        }
    }
}
