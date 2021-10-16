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
        public static bool GetIsLoggedIn(this ClaimsPrincipal principal)
        {
            return principal.Claims.Any();
        }
        public static int GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            if (!SessionUtils.GetIsLoggedIn(principal))
            {
                return -1;
            }
            return int.Parse(principal.Claims.First().Value);
        }
        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            return SessionUtils.GetIsLoggedIn(principal) && principal.IsInRole(nameof(UserType.Admin));
        }
    }
}
