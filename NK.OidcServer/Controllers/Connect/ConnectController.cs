using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;

namespace NK.OidcServer.Controllers.Connect
{
    [Route("api/[controller]")]
    public class ConnectController: ControllerBase
    {
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutApi()
        {
            // 1) Cookie 認証を破棄（ブラウザ/SPAのログイン状態）
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // 2) OIDCサーバー側のセッションも破棄（必要な場合）
            await HttpContext.SignOutAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            return Ok(new { message = "signed out" });
        }
    }
}
