using Domain.Entities;
using Identites.Core.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services;

public class SignInService(SignInManager<AppUser, Guid> signInManager)
{
}
