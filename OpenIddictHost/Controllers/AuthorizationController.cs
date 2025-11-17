using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace OpenIddictHost.Controllers;

public class AuthorizationController : Controller
{
    [HttpGet("authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("OpenIddict request is missing.");
        if(!User!.Identity!.IsAuthenticated is not true)
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Request.Path + QueryString.Create(Request.Query.ToList())
            }, "Cookies");
        }
        var subject = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? throw new InvalidOperationException("User id not found.");

        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            nameType: OpenIddictConstants.Claims.Name,
            roleType: OpenIddictConstants.Claims.Role);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, subject);
        identity.SetClaim(OpenIddictConstants.Claims.Name, User.Identity!.Name ?? "");

        identity.SetDestinations(claim => claim.Type switch
        {
            OpenIddictConstants.Claims.Name => new[]
            {
                OpenIddictConstants.Destinations.AccessToken,
                OpenIddictConstants.Destinations.IdentityToken
            },
            _ => new[] { OpenIddictConstants.Destinations.AccessToken }
        });

        var principal = new ClaimsPrincipal(identity);

        // 要求された scope を付与
        principal.SetScopes(request.GetScopes());

        // ここで同意画面を挟むなら、
        // 一旦 pending 状態にして Razor ページへ出す、などの実装も可能

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
