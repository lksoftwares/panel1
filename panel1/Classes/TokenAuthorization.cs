//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Primitives;

//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class TokenAuthorizationAttribute : Attribute, IAuthorizationFilter
//{
//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        StringValues authHeader;
//        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out authHeader))
//        {
//            context.Result = new UnauthorizedResult();
//            return;
//        }

//        string token = authHeader.ToString().Split(" ").LastOrDefault();

//        if (string.IsNullOrEmpty(token) || !IsValidToken(token))
//        {
//            context.Result = new UnauthorizedResult();
//        }
//    }

//    private bool IsValidToken(string token)
//    {
        
//        var validTokens = new List<string> { "2Fsk5LBU5j1DrPldtFmLWeO8uZ8skUzwhe3ktVimUE8=" };
//        return validTokens.Contains(token);
//    }
//}
