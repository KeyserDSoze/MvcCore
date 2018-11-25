using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.Middleware.CustomMiddleware
{
    /// <summary>
    /// This is a class that can substitute the standard MiddlewareFactory.
    /// https://simpleinjector.org/index.html
    /// </summary>
    public class CustomMiddlewareFactory : IMiddlewareFactory
    {
        private readonly IOptions<QuerystringBehaviorOptions> options;

        public CustomMiddlewareFactory(IOptions<QuerystringBehaviorOptions> options)
        {
            this.options = options;
        }
        public IMiddleware Create(Type middlewareType)
        {
            Console.WriteLine($"Create middleware {middlewareType.FullName}");
            //Do some different work (different from standard)
            //Create the instance for factory
            return Activator.CreateInstance(middlewareType, new object[1] { options }) as IMiddleware; //in this case with options parameter (this is an optional)
        }

        public void Release(IMiddleware middleware)
        {
            Console.WriteLine($"Release middleware {middleware.GetType().FullName}");
            //Do some different work (different from standard)
            //The container is responsible for releasing resources.
        }
    }
}
