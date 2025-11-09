using System.ComponentModel.DataAnnotations;

namespace HostApp.Controllers.Requests;
/// <summary>
/// /account/login に POST される資格情報DTO。
/// </summary>
public sealed class LoginRequest
{
    /// <summary>ユーザー名（またはログインID/メールアドレス）</summary>
    [Required, StringLength(256)]
    public string UserName { get; set; } = string.Empty;

    /// <summary>平文パスワード（TLS 前提）</summary>
    [Required, StringLength(256, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>ログイン後に戻すURL（ローカルURLのみ許可）</summary>
    [StringLength(1024)]
    public string? ReturnUrl { get; set; }

    /// <summary>永続Cookieの希望（Remember me）</summary>
    public bool RememberMe { get; set; } = false;
}