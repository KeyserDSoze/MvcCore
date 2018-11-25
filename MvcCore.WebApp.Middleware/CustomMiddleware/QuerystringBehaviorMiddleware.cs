﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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

        public QuerystringBehaviorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var parameter = context.Request.Query["id"];
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                //make something special here
            }

            // Call the next delegate/middleware in the pipeline
            await this.next(context);
        }
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
