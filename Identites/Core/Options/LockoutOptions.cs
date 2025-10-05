using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>ロックアウト設定（ASP.NET Identity 既定に準拠）</summary>
    public sealed class LockoutOptions
    {
        /// <summary>最大失敗回数（既定: 5）</summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;

        /// <summary>ロックアウト持続時間（既定: 5分）</summary>
        public TimeSpan DefaultLockoutTimeSpan { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>新規ユーザーにロックアウトを適用するか（既定: true）</summary>
        public bool AllowedForNewUsers { get; set; } = true;
    }
}
