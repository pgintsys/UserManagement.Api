using System.Net;
using System.Net.Mail;
using Emte.Core.Models.Request;
using Microsoft.AspNetCore.Http;

namespace Emte.Core.API.Extenstions
{
    public static class HttpContextExtensions
    {
        public static bool IsSuperAdmin(this HttpContext httpContext)
        {
            if (httpContext.User == null) { return false; }
            
            var roles = httpContext.User.Claims.FirstOrDefault(c => c.Type == "Role");
            if (roles == null || roles.Value == null) { return false; }
            
            var roleValue = roles.Value as string;
            if (roleValue == null) { return false; }
            var roleCollection = roleValue.Split(",");
            return roleCollection.Any(r => r == Contants.Roles.SuperAdmin);
        }
    }
}