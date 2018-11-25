using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.Middleware.CustomMiddleware
{
    /// <summary>
    /// IMiddleware is based on "Factory pattern". MiddlewareFactory is the name of activator.
    /// </summary>
    public class MiddlewareAsService : IMiddleware
    {
        private readonly IOptions<QuerystringBehaviorOptions> options;

        public MiddlewareAsService( IOptions<QuerystringBehaviorOptions> options)
        {
            this.options = options;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Console.WriteLine("Middleware as Service. Usually transient service");
            var parameter = context.Request.Query[this.options.Value.Option1];
            Console.WriteLine($"Try to get parameter from {this.options.Value.Option1}");
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                int b = this.options.Value.Option2;
                Console.WriteLine($"Using Option2 {b}");
                //make something special here
            }
            Console.WriteLine($"Call next Middleware {next.Target.GetType().FullName}");
            await next(context);
        }
    }
    public static class MiddlewareAsServiceExtension
    {
        public static IApplicationBuilder UseFactoryActivatedMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareAsService>();
        }
    }
}
