using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Models
{
    public enum SignInResult
    {
        Success,
        Failed,
        LockedOut,
        RequiresTwoFactor
    }
}
