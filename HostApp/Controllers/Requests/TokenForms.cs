using System.ComponentModel.DataAnnotations;

namespace HostApp.Controllers.Requests;
public abstract class TokenFormBase
{
    [Required] public string grant_type { get; set; } = "";
}

public sealed class AuthorizationCodeTokenForm : TokenFormBase
{
    [Required] public string code { get; set; } = "";
    [Required] public string redirect_uri { get; set; } = "";
    [Required] public string code_verifier { get; set; } = "";
}

public sealed class RefreshTokenForm : TokenFormBase
{
    [Required] public string refresh_token { get; set; } = "";
}
