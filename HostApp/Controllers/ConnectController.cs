using Core.Models;
using Core.Services;
using HostApp.Controllers.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostApp.Controllers
{
    [ApiController]
    [Route("connect")]
    public class ConnectController : ControllerBase
    {
        private readonly AuthorizationService _authz;
        private readonly TokenService _tokens;

        public ConnectController(AuthorizationService authz, TokenService tokens)
        { _authz = authz; _tokens = tokens; }

        // GET /connect/authorize
        [HttpGet("authorize")]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize([FromQuery] AuthorizeQuery q, CancellationToken ct)
        {
            var req = new AuthorizeRequest(
                q.response_type, q.client_id, q.redirect_uri, q.scope,
                q.state, q.code_challenge, q.code_challenge_method, q.nonce
            );

            var outcome = await _authz.AuthorizeAsync(req, User, HttpContext.Session?.Id, ct);
            return outcome switch
            {
                // 認可コード発行 → クライアントへ
                RedirectOutcome r => Redirect(r.RedirectUri),

                // 👇 AuthorizationService が「未ログイン」と判断（login_required）した場合の処理
                // Cookie 認証を使ってるなら Challenge で LoginPath に飛ぶ（returnUrl を現在URLに設定）
                ErrorOutcome { Error: "login_required" } => Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(HttpContext.Request)
                    },
                    "Cookies" // ← クッキー認証のスキーム名
                ),

                // その他のバリデーションエラー
                ErrorOutcome e => BadRequest(new { error = e.Error, error_description = e.Description }),
                _ => BadRequest(new { error = "invalid_request" })
            };
        }

        // POST /connect/token
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Token([FromForm] TokenFormBase formBase, CancellationToken ct)
        {
            // DataAnnotations の Required を使うため、具象型に再バインド
            if (string.Equals(formBase.grant_type, "authorization_code", StringComparison.Ordinal))
            {
                var form = new AuthorizationCodeTokenForm();
                await TryUpdateModelAsync(form); // FromForm 再バインド
                if (!ModelState.IsValid) return BadRequest(new { error = "invalid_request" });

                var grant = new AuthCodeGrant(form.code, form.redirect_uri, form.code_verifier);
                var res = await _tokens.HandleAuthorizationCodeAsync(grant, ct);
                return res switch
                {
                    TokenSuccess ok => Ok(new
                    {
                        access_token = ok.AccessToken,
                        token_type = "Bearer",
                        expires_in = ok.ExpiresIn,
                        id_token = ok.IdToken,
                        refresh_token = ok.RefreshToken
                    }),
                    TokenError er => BadRequest(new { error = er.Error, error_description = er.Description }),
                    _ => BadRequest(new { error = "invalid_request" })
                };
            }

            if (string.Equals(formBase.grant_type, "refresh_token", StringComparison.Ordinal))
            {
                var form = new RefreshTokenForm();
                await TryUpdateModelAsync(form);
                if (!ModelState.IsValid) return BadRequest(new { error = "invalid_request" });

                // TODO: Refresh 実装を TokenService に追加して呼び出す
                return BadRequest(new { error = "unsupported_grant_type" });
            }

            return BadRequest(new { error = "unsupported_grant_type" });
        }

        // GET /connect/userinfo
        [HttpGet("userinfo")]
        [Authorize] // Bearer でも Cookie でも可（ポリシーは環境に合わせる）
        public IActionResult UserInfo()
        {
            var sub = User.FindFirst("sub")?.Value ?? string.Empty;
            return Ok(new { sub });
        }
    }
}
