using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>パスワードのポリシー設定（ASP.NET Identity 既定に準拠）</summary>
    public sealed class PasswordOptions
    {
        /// <summary>最低文字数（既定: 6）</summary>
        public int RequiredLength { get; set; } = 6;

        /// <summary>必要な異なる文字の最小数（既定: 1）</summary>
        public int RequiredUniqueChars { get; set; } = 1;

        /// <summary>数字を必須にするか（既定: true）</summary>
        public bool RequireDigit { get; set; } = true;

        /// <summary>小文字を必須にするか（既定: false）</summary>
        public bool RequireLowercase { get; set; } = false;

        /// <summary>大文字を必須にするか（既定: false）</summary>
        public bool RequireUppercase { get; set; } = false;

        /// <summary>英数字以外（記号）を必須にするか（既定: false）</summary>
        public bool RequireNonAlphanumeric { get; set; } = false;

        /// <summary>直近N件のパスワード再利用を禁止（0なら無効）</summary>
        public int DisallowReuseLast { get; set; } = 0;
    }
}
