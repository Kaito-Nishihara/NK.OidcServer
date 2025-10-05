using Identites.Core.Abstractions;
using Identites.Core.Crypto;
using Identites.Core.Options;
using Identites.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Extensions
{
    /// <summary>
    /// Identity Core の登録拡張（EF など外部依存は含めない）
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// ユーザー/ロール/キー型を指定して Identity Core を登録します。
        /// - Options（<see cref="OidcIdentityOptions"/>）の構成
        /// - 既定サービス（Hasher/Normalizer/TOTP/Clock）
        /// - Managers（User/Role/SignIn）
        /// </summary>
        public static OidcIdentityBuilder<TUser, TRole, TKey> AddOidcIdentityCore<TUser, TRole, TKey>(
            this IServiceCollection services,
            Action<OidcIdentityOptions>? configure = null)
            where TUser : class, IAuthUser<TKey>
            where TRole : class, IAuthRole<TKey>
        {
            // Options 登録（Configure を呼べば AddOptions は自動で入る）
            if (configure is not null) services.Configure(configure);
            else services.AddOptions<OidcIdentityOptions>();

            // ===== 既定の実装を DI に登録（TryAdd で上書き可能） =====
            services.TryAddSingleton<ILookupNormalizer, UpperInvariantNormalizer>();
            services.TryAddSingleton<IClock, SystemClock>();
            services.TryAddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
            services.TryAddSingleton<ITotpProvider, TotpProvider>();

            // ===== Managers =====
            services.TryAddScoped<UserManager<TUser, TKey>>();
            services.TryAddScoped<RoleManager<TRole, TKey>>();
            services.TryAddScoped<SignInManager<TUser, TKey>>();

            return new OidcIdentityBuilder<TUser, TRole, TKey>(services);
        }

        /// <summary>
        /// 実行時に Options 値へアクセスしたい場合に便利（IOptionsMonitor で監視）。
        /// </summary>
        public static IServiceCollection PostConfigureIdentity(
            this IServiceCollection services,
            Action<OidcIdentityOptions> postConfigure)
        {
            services.PostConfigure(postConfigure);
            return services;
        }
    }

    /// <summary>
    /// 後続のチェーン拡張を受けるための最小ビルダー。
    /// EF ストアやトークン発行（JWT/Cookie）は別拡張でこのビルダーにぶら下げます。
    /// </summary>
    public sealed class OidcIdentityBuilder<TUser, TRole, TKey>
        where TUser : class, IAuthUser<TKey>
        where TRole : class, IAuthRole<TKey>
    {
        public IServiceCollection Services { get; }

        internal OidcIdentityBuilder(IServiceCollection services) => Services = services;

        /// <summary>
        /// 追加のパスワードバリデータ/ユーザーバリデータ等をまとめて登録するユーティリティ（任意）。
        /// </summary>
        public OidcIdentityBuilder<TUser, TRole, TKey> ConfigureValidators<TPasswordValidator, TUserValidator>()
            where TPasswordValidator : class
            where TUserValidator : class
        {
            Services.AddScoped(typeof(TPasswordValidator));
            Services.AddScoped(typeof(TUserValidator));
            return this;
        }

        /// <summary>
        /// 既定のトークンプロバイダを登録（Core 側では TOTP のみ）。
        /// メール確認/パスワードリセットの DataProtection ベースは Infra 側で拡張してください。
        /// </summary>
        public OidcIdentityBuilder<TUser, TRole, TKey> AddDefaultTokenProviders()
        {
            // 既に TryAdd 済み（TotpProvider）。拡張ポイントだけ残す。
            return this;
        }
    }
}
