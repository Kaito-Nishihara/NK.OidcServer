using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace OpenIddictHost.Extensions;
public static class OpenIddictControllerExtensions
{
    /// <summary>
    /// OpenIddict 用のエラー付き Forbid 結果を返します。
    /// </summary>
    /// <param name="controller">コントローラー。</param>
    /// <param name="error">エラーコード（例: Errors.LoginRequired）。</param>
    /// <param name="errorDescription">エラー説明メッセージ。</param>
    /// <returns>ForbidResult。</returns>
    public static IActionResult ForbidOidc(
        this ControllerBase controller,
        string error,
        string? errorDescription = null)
    {
        var dict = new Dictionary<string, string?>
        {
            [OpenIddictServerAspNetCoreConstants.Properties.Error] = error
        };

        if (!string.IsNullOrEmpty(errorDescription))
        {
            dict[OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = errorDescription;
        }

        var props = new AuthenticationProperties(dict);

        return controller.Forbid(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: props);
    }
}