using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Extensions
{
    public static class ExtensionMethods
    {
        public static List<string>? GetRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.Select(x => x.Value).ToList();
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new(ClaimTypes.Name, name));
        }
        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new(ClaimTypes.NameIdentifier, nameIdentifier));
        }
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            claims.Add(new(ClaimTypes.Email, email));
        }
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(x => claims.Add(new(ClaimTypes.Role, x)));
        }


    }
}
