namespace Core.Models;
public sealed record AuthorizeRequest(
    string ResponseType, string ClientId, string RedirectUri, string Scope,
    string State, string CodeChallenge, string CodeChallengeMethod, string? Nonce);

public abstract record AuthorizeOutcome;
public sealed record RedirectOutcome(string RedirectUri) : AuthorizeOutcome;
public sealed record ErrorOutcome(string Error, string? Description = null) : AuthorizeOutcome;