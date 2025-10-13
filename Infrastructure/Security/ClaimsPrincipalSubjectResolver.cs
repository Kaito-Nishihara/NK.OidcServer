using Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;
public sealed class ClaimsPrincipalSubjectResolver : ISubjectResolver
{
    public string? ResolveSubject(ClaimsPrincipal user)
    {
        // 一般的な優先順：sub → NameIdentifier → Name
        return user.FindFirst("sub")?.Value
            ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.Identity?.Name;
    }
}