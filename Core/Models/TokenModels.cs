using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;
public abstract record TokenOutcome;
public sealed record TokenSuccess(string AccessToken, int ExpiresIn, string? IdToken, string? RefreshToken) : TokenOutcome;
public sealed record TokenError(string Error, string? Description = null) : TokenOutcome;

public sealed record AuthCodeGrant(string Code, string RedirectUri, string CodeVerifier);
public sealed record RefreshGrant(string RefreshToken);