using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCore.WebApp.DependencyInjection.Code
{
    public class MySingletonInjection
    {
        public Guid Guid => Guid.NewGuid();
    }
    public abstract class AMyTransientInjection
    {
        public Guid Guid => Guid.NewGuid();
    }
    public class MyTransientInjection : AMyTransientInjection { }
    public abstract class AMyScopedInjection
    {
        public Guid Guid => Guid.NewGuid();
    }
    public class MyScopedInjection : AMyScopedInjection { }
    public interface IInjectionObserver { }
    public class InjectionObserver : IInjectionObserver
    {
        public InjectionObserver(
            IApplicationBuilderFactory applicationBuilderFactory, //Transient
            IApplicationLifetime applicationLifetime,             //Singleton
            IHostingEnvironment hostingEnvironment,               //Singleton
            IStartup startup,                                     //Singleton
            IStartupFilter startupFilter,                         //Transient
            IServer server,                                       //Singleton
            IHttpContextFactory httpContextFactory,               //Transient
            ILogger<InjectionObserver> logger,                    //Singleton
            ILoggerFactory loggerFactory,                         //Singleton
            ObjectPoolProvider objectPoolProvider,                //Singleton
            //IConfigureOptions<InjectionObserver> configureOptions,//Transient
            //IOptions<InjectionObserver> options,                //Singleton
            DiagnosticSource diagnosticSource,                    //Singleton
            DiagnosticListener diagnosticListener,                //Singleton
            MySingletonInjection mySingletonInjection,            //Singleton
            AMyTransientInjection myTransientInjection,            //Transient
            AMyScopedInjection myScopedInjection                   //Scoped
            )
        {
            //Set a breakpoint here to see the magic, first step
            Console.WriteLine(mySingletonInjection.Guid);
            Console.WriteLine(myTransientInjection.Guid);
            Console.WriteLine(myScopedInjection.Guid);
            string dsak;
        }
    }
}
