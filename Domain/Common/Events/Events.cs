using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Events
{
    /// <summary>
    /// ドメインイベントのマーカーインターフェース
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// イベントが発生した時刻（UTC）
        /// </summary>
        DateTime OccurredOn { get; }
    }
}
