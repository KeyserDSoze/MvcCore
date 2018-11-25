using System;
using System.Collections.Generic;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcCore.WebApp.Middleware.CustomMiddleware;

namespace MvcCore.WebApp.Middleware
{
    public class Startup
    {
        //To use container you must install nuget "SimpleInjector" to customize IMiddlewareFactory
        private Container container = new Container();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //Configure DI for QuerystringBehaviorMiddleware
            services.Configure<QuerystringBehaviorOptions>(Configuration.GetSection("QuerystringBehaviorOptions"));

            //Configure Middleware with Middleware Factory
            services.AddTransient<MiddlewareAsService>();

            // Replace the default middleware factory with the CustomMiddlewareFactory. To use container you must install nuget "SimpleInjector"
            services.AddTransient<IMiddlewareFactory>(_ =>
            {
                return new CustomMiddlewareFactory(container);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        /// <summary>
        /// This method is called first time to register the chain, every request uses this chain (it's based on chain of responsibility pattern). This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <Middleware name="Authentication" order="Before HttpContext.User is needed. Terminal for OAuth callbacks.">Provides authentication support.</Middleware>
        /// <Middleware name="Cookie Policy" order="Before middleware that issues cookies. Examples: Authentication, Session, MVC (TempData).">Tracks consent from users for storing personal information and enforces minimum standards for cookie fields, such as secure and SameSite.</Middleware>
        /// <Middleware name="CORS" order="Before components that use CORS.">Configures Cross-Origin Resource Sharing.</Middleware>
        /// <Middleware name="Diagnostics" order="Before components that generate errors.">Configures diagnostics.</Middleware>
        /// <Middleware name="Forwarded Headers" order="Before components that consume the updated fields. Examples: scheme, host, client IP, method.">Forwards proxied headers onto the current request.</Middleware>
        /// <Middleware name="HTTP Method Override" order="Before components that consume the updated method.">Allows an incoming POST request to override the method.</Middleware>
        /// <Middleware name="HTTPS Redirection" order="Before components that consume the URL.">Redirect all HTTP requests to HTTPS (ASP.NET Core 2.1 or later).</Middleware>
        /// <Middleware name="HTTP Strict Transport Security (HSTS)" order="Before responses are sent and after components that modify requests. Examples: Forwarded Headers, URL Rewriting.">Security enhancement middleware that adds a special response header (ASP.NET Core 2.1 or later).</Middleware>
        /// <Middleware name="MVC" order="Terminal if a request matches a route.">Processes requests with MVC/Razor Pages (ASP.NET Core 2.0 or later).</Middleware>
        /// <Middleware name="OWIN" order="Terminal if the OWIN Middleware fully processes the request.">Interop with OWIN-based apps, servers, and middleware.</Middleware>
        /// <Middleware name="Response Caching" order="Before components that require caching.">Provides support for caching responses.</Middleware>
        /// <Middleware name="Response Compression" order="Before components that require compression.">Provides support for compressing responses.</Middleware>
        /// <Middleware name="Request Localization" order="Before localization sensitive components.">Provides localization support.</Middleware>
        /// <Middleware name="Routing" order="Terminal for matching routes.">Defines and constrains request routes.</Middleware>
        /// <Middleware name="Session" order="Before components that require Session.">Provides support for managing user sessions.</Middleware>
        /// <Middleware name="Static Files" order="Terminal if a request matches a file.">Provides support for serving static files and directory browsing.</Middleware>
        /// <Middleware name="URL Rewriting" order="Before components that consume the URL.">Provides support for rewriting URLs and redirecting requests.</Middleware>
        /// <Middleware name="WebSockets" order="Before components that are required to accept WebSocket requests.">Enables the WebSockets protocol.</Middleware>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //This Middleware adds a custom error page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //This Middleware adds a redirect from http to https
            app.UseHttpsRedirection();
            //This Middleware enables static file
            app.UseStaticFiles();
            //This Middleware adds Cookie policy
            app.UseCookiePolicy();

            //I want to set at this point my custom Middleware by delegation
            app.Use(async (context, next) =>
            {
                // Do something
                await next.Invoke();
                // You can do somethingelse in the end of chain, but it's not a best practice.
                // If you want to add something in the end, you should use another middleware with app.Run
            });

            //Middleware for MiddlewareFactory registration
            app.UseFactoryActivatedMiddleware();

            //I want to set at this point my custom Middleware created in CustomMiddleware
            app.UseQuerystringBehavior();

            //This Middleware adds routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Using run to finalize the chain, if run is never called the chain anyway ends. In HandleIndex you'll find app.Run
            app.Map("/Home", HandleIndex); //If the request is based on action Index, the final run is in HandleHome and the next run is skipped.

            //MapWhen if you want to check something more complex than path
            app.MapWhen(context => context.Request.Query.ContainsKey("id"),
                               HandleParameterId);
            
            //Nesting is supported
            app.Map("/level1", level1App => {
                level1App.Map("/level2a", level2AApp => {
                    // "/level1/level2a" processing
                    Console.WriteLine("level2a in level 1");
                });
                level1App.Map("/level2b", level2BApp => {
                    // "/level1/level2b" processing
                    Console.WriteLine("level2b in level 1");
                });
            });

            app.Run(async context =>
            {
                Console.WriteLine("This is something else");
                await context.Response.WriteAsync("<h1>This is something else</h1>");
                //Do something, for example your custom log, but it's not a best practice to do log here.
                //You should use ILogger or ILoggerProvider and create your custom provider like in "03 - MvcCore.WebApp.CustomLog" project
            });
        }
        private static void HandleIndex(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                Console.WriteLine("This is the Index");
                await context.Response.WriteAsync("<h1>This is the Index</h1>");
            });
        }
        private static void HandleParameterId(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                Console.WriteLine("There's an Id");
                await context.Response.WriteAsync("<h1>There's an Id</h1>");
            });
        }
    }
}
