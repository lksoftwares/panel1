using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace panel1.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class newMiddleware
    {
        private readonly RequestDelegate _next;

        public newMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class newMiddlewareExtensions
    {
        public static IApplicationBuilder UsenewMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<newMiddleware>();
        }
    }
}
