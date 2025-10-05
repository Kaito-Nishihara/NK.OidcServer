using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>Identity全体の構成ルート。DIで IOptions&lt;OidcIdentityOptions&gt; として参照。</summary>
    public sealed class OidcIdentityOptions
    {
        public PasswordOptions Password { get; } = new();
        public LockoutOptions Lockout { get; } = new();
        public SignInOptions SignIn { get; } = new();
        public UserOptions User { get; } = new();
        public TokenOptions Tokens { get; } = new();

        /// <summary>ユーザー名/メールの正規化を大文字化等で行うか（既定: true）</summary>
        public bool UseDefaultNormalizer { get; set; } = true;

        /// <summary>セキュリティスタンプ検証の最小間隔（Cookie再検証などで使用 / 既定: 30分）</summary>
        public TimeSpan SecurityStampValidationInterval { get; set; } = TimeSpan.FromMinutes(30);
    }
}
