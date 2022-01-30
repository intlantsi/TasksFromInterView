using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CSBI.Middlewares
{
    public class SecureAdminMiddleware
    {
        static string header= "secure-admin";
        static string id = "e98d2243-8fd5-40f6-990f-48ca312b8aa5";
        private readonly RequestDelegate next;

        public SecureAdminMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(CheckContext(context))
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("Id", id) }, "AuthenticationTypes.Federation"));
            }
            await next(context);
        }

        public static bool CheckContext(HttpContext context)
        {
            return context.Request.Headers.ContainsKey(header) &&
                   context.Request.Headers[header].Equals(id);
        }
    }
}
