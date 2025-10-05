using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>ユーザー名/メール関連の設定（ASP.NET Identity 既定に準拠）</summary>
    public sealed class UserOptions
    {
        /// <summary>許可されるユーザー名の文字</summary>
        public string AllowedUserNameCharacters { get; set; }
            = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        /// <summary>メールアドレスを一意にするか（既定: false）</summary>
        public bool RequireUniqueEmail { get; set; } = false;
    }
}
