using FireManagerServer.Service.JwtService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FireManagerServer.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifyTokenAttribute: Attribute, IAuthorizationFilter
    {
    
        private readonly IJwtService _jwtService = new JwtService(new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build());
        public VerifyTokenAttribute()
        {
           
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
           
            var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length);
                var claims = _jwtService.VerifyToken(token);
                if(claims==null)
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
