using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>サインイン挙動の設定</summary>
    public sealed class SignInOptions
    {
        /// <summary>メール確認が必須か（既定: false）</summary>
        public bool RequireConfirmedEmail { get; set; } = false;

        /// <summary>電話番号確認が必須か（既定: false）</summary>
        public bool RequireConfirmedPhoneNumber { get; set; } = false;

        /// <summary>アカウント確認（メール等）が必須か（既定: false）</summary>
        public bool RequireConfirmedAccount { get; set; } = false;

        /// <summary>2要素認証を全体で必須にするか（既定: false）</summary>
        public bool RequireTwoFactor { get; set; } = false;
    }
}
