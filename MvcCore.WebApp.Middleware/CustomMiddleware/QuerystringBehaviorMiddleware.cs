using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.Middleware.CustomMiddleware
{
    //It's based on "Chain Of Responsibility" pattern
    //https://alessandrorapiti.com/Home/LetMeSee/development/1/1/2a1aae67427444e3bc0185b25c898176
    public class QuerystringBehaviorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IOptions<QuerystringBehaviorOptions> options;

        public QuerystringBehaviorMiddleware(RequestDelegate next, IOptions<QuerystringBehaviorOptions> options)
        {
            this.next = next;
            this.options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var parameter = context.Request.Query[this.options.Value.Option1];
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                int b = int.Parse(this.options.Value.Option2);
                //make something special here
            }

            // Call the next delegate/middleware in the pipeline
            await this.next(context);
        }
    }
    public class QuerystringBehaviorOptions
    {
        public string Option1 { get; set; }
        public string Option2 { get; set; }
    }
    public static class QuerystringBehaviorMiddlewareExtension
    {
        public static IApplicationBuilder UseQuerystringBehavior(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QuerystringBehaviorMiddleware>();
        }
    }
}
