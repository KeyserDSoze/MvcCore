using Microsoft.AspNetCore.Http;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.Middleware.CustomMiddleware
{
    /// <summary>
    /// This is a class that can substitute the standard MiddlewareFactory. To create this one, you must install from nuget "SimpleInjector" (for Container)
    /// https://simpleinjector.org/index.html
    /// </summary>
    public class CustomMiddlewareFactory : IMiddlewareFactory
    {
        private readonly Container container;

        public CustomMiddlewareFactory(Container container)
        {
            this.container = container;
        }
        public IMiddleware Create(Type middlewareType)
        {
            //Do some different work (different from standard)
            return container.GetInstance(middlewareType) as IMiddleware;
        }

        public void Release(IMiddleware middleware)
        {
            //Do some different work (different from standard)
            //The container is responsible for releasing resources.
        }
    }
}
