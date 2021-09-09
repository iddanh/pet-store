using pet_store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pet_store.Services
{
    public static class SessionUtils
    {
        public static int GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Claims.First().Value);
        }
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return principal.IsInRole(nameof(UserType.Admin));
        }
    }
}
