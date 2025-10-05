using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static System.Formats.Asn1.AsnWriter;

namespace OidcServer.Controllers
{
    public class ConnectController(UserManager<User>) : ControllerBase
    {
        [HttpGet("~/connect/authorize"), HttpPost("~/connect/authorize"), Produces("application/json")]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            var cookieAuth = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (cookieAuth is not { Succeeded: true })
            {
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(Request.Query)
                }, IdentityConstants.ApplicationScheme);
            }

            var user = await userManager.GetUserAsync(cookieAuth.Principal!);
            if (user is null)
            {
                await signInManager.SignOutAsync();
                return Challenge(new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                        Request.HasFormContentType ? Request.Form : Request.Query)
                });
            }
            var application = await applicationManager.FindByClientIdAsync(request.ClientId!)
                ?? throw new InvalidOperationException("Client application cannot be found.");

            var authorizations = await authorizationManager.FindAsync(
                subject: await userManager.GetUserIdAsync(user),
                client: await applicationManager.GetIdAsync(application),
                status: Statuses.Valid,
                type: AuthorizationTypes.Permanent,
                scopes: request.GetScopes()).ToListAsync();

            var identity = new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: Claims.Name,
                roleType: Claims.Role);

            var requested = request.GetScopes();
            var allowed = new[] { Scopes.OpenId, Scopes.Profile, Scopes.Email, Scopes.Phone, Scopes.Address, Scopes.Roles, Scopes.OfflineAccess };
            var granted = requested.Intersect(allowed).ToImmutableArray();
            identity.SetScopes(granted);

            identity.SetClaim(Claims.Subject, await userManager.GetUserIdAsync(user));

            // profile スコープのときだけ name/username を積む
            if (identity.HasScope(Scopes.Profile))
            {
                var uname = user.UserName ?? string.Empty;
                identity.SetClaim(Claims.Name, uname);
                identity.SetClaim(Claims.PreferredUsername, uname);
            }

            // email スコープのときだけ
            if (identity.HasScope(Scopes.Email))
            {
                identity.SetClaim(Claims.Email, await userManager.GetEmailAsync(user));
                identity.SetClaim(Claims.EmailVerified, (await userManager.IsEmailConfirmedAsync(user)).ToString().ToLowerInvariant());
            }

            // phone スコープのときだけ
            if (identity.HasScope(Scopes.Phone))
            {
                var phone = await userManager.GetPhoneNumberAsync(user);
                if (!string.IsNullOrWhiteSpace(phone))
                    identity.SetClaim(Claims.PhoneNumber, phone);
                identity.SetClaim(Claims.PhoneNumberVerified, (await userManager.IsPhoneNumberConfirmedAsync(user)).ToString().ToLowerInvariant());
            }

            // address スコープのときだけ（Cookie 側に既に載っていれば流用。なければ省略）
            if (identity.HasScope(Scopes.Address))
            {
                var addrRaw = cookieAuth.Principal?.FindFirst(Claims.Address)?.Value;
                if (!string.IsNullOrWhiteSpace(addrRaw))
                    identity.SetClaim(Claims.Address, addrRaw);
            }

            // roles スコープのときだけ
            if (identity.HasScope(Scopes.Roles))
            {
                var roles = await userManager.GetRolesAsync(user);
                identity.SetClaims(Claims.Role, roles.ToImmutableArray());
            }

            // リソース解決
            identity.SetResources(await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

            var authorization = authorizations.LastOrDefault();
            authorization ??= await authorizationManager.CreateAsync(
                identity: identity,
                subject: await userManager.GetUserIdAsync(user),
                client: (await applicationManager.GetIdAsync(application))!,
                type: AuthorizationTypes.Permanent,
                scopes: identity.GetScopes());

            identity.SetAuthorizationId(await authorizationManager.GetIdAsync(authorization));

            var principal = new ClaimsPrincipal(identity);
            identity.SetDestinations(claim => GetDestinations(claim, principal));
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            switch (claim.Type)
            {
                case Claims.Subject:
                    yield return Destinations.AccessToken;
                    yield return Destinations.IdentityToken;
                    yield break;

                case Claims.Name:
                case Claims.PreferredUsername:
                    if (principal.HasScope(Scopes.Profile))
                    {
                        yield return Destinations.AccessToken;
                        yield return Destinations.IdentityToken;
                    }
                    yield break;

                case Claims.Email:
                case Claims.EmailVerified:
                    if (principal.HasScope(Scopes.Email))
                    {
                        yield return Destinations.AccessToken;
                        yield return Destinations.IdentityToken;
                    }
                    yield break;

                case Claims.PhoneNumber:
                case Claims.PhoneNumberVerified:
                    if (principal.HasScope(Scopes.Phone))
                    {
                        yield return Destinations.AccessToken;
                        yield return Destinations.IdentityToken;
                    }
                    yield break;

                case Claims.Address:
                    if (principal.HasScope(Scopes.Address))
                    {
                        yield return Destinations.AccessToken;
                        yield return Destinations.IdentityToken;
                    }
                    yield break;

                case Claims.Role:
                    if (principal.HasScope(Scopes.Roles))
                    {
                        yield return Destinations.AccessToken;
                        yield return Destinations.IdentityToken;
                    }
                    yield break;

                // 既定は何にも載せない（最小権限）
                default:
                    yield break;
            }
        }
    }
}
