using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identites.Core.Abstractions
{
    public static class AuthErrors
    {
        public const string UserNameRequired = "Username is required.";
        public const string PasswordRequired = "Password is required.";
        public const string DuplicateUserName = "Username already exists.";
        public const string DuplicateEmail = "Email already exists.";
        public const string InvalidCredentials = "Invalid username or password.";
        public const string LockedOut = "User is locked out.";
        public const string RequiresTwoFactor = "Two-factor authentication required.";
    }
}
