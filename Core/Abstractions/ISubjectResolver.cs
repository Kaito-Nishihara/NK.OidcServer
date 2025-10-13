using System.Security.Claims;

namespace Core.Abstractions;
public interface ISubjectResolver
{
    /// <summary>Cookieサインイン済みのユーザーPrincipalから "sub" を取り出す。</summary>
    string? ResolveSubject(ClaimsPrincipal user);
}