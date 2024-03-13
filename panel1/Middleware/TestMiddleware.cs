//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//namespace panel1.Middleware
//{
//    public class TestMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public TestMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext httpContext)
//        {
//            //if (httpContext.Response.StatusCode >= 199)
//            if (!context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))

//            {
//                Console.WriteLine("Middleware executed ");

//                // Console.WriteLine($"Error in response. Status Code: {httpContext.Response.StatusCode}");
//            }

//            await _next(httpContext);
//        }
//    }

//    public static class TestMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<TestMiddleware>();
//        }
//    }
//}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace panel1.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.Equals("/api/Login",StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Middleware executed ");
            }

            await _next(httpContext);
        }
    }

    public static class TestMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMiddleware>();
        }
    }
}
