using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Options
{
    /// <summary>各種トークンの寿命やプロバイダ名などを束ねる設定</summary>
    public sealed class TokenOptions
    {
        /// <summary>メール確認トークンの有効期限（既定: 1日）</summary>
        public TimeSpan EmailConfirmationTokenLifespan { get; set; } = TimeSpan.FromDays(1);

        /// <summary>パスワードリセットトークンの有効期限（既定: 1日）</summary>
        public TimeSpan PasswordResetTokenLifespan { get; set; } = TimeSpan.FromDays(1);

        /// <summary>2FA（TOTPやSMSコード）の有効期限（既定: 5分）</summary>
        public TimeSpan TwoFactorTokenLifespan { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>JWTリフレッシュトークンの有効期限（既定: 14日）</summary>
        public TimeSpan RefreshTokenLifespan { get; set; } = TimeSpan.FromDays(14);

        /// <summary>JWTアクセストークンの有効期限（既定: 30分）</summary>
        public TimeSpan AccessTokenLifespan { get; set; } = TimeSpan.FromMinutes(30);
    }
}
