using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcCore.WebApp.Options.Code;

namespace MvcCore.WebApp.Options
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. (CONFIG BUILDER NOT MODIFIED)
        public void ConfigureServices2(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<OptionRoot>(Configuration);
            services.Configure<SubOption>(Configuration.GetSection("Subsection"));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        // This method gets called by the runtime.Use this method to add services to the container. (CONFIG BUILDER MODIFIED)
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var configBuilder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true)
           .AddJsonFile("appsettings2.json", optional: true);
            var config = configBuilder.Build();

            services.Configure<OptionRoot>(config);
            //it's possible to configure a dictionary of same configurations
            services.Configure<OptionRoot>("firstConfig", config);
            services.Configure<SubOption>(config.GetSection("Subsection"));
            services.Configure<OptionRoot2>(config);
            services.Configure<SubOption2>(config.GetSection("Subsection2"));
            //Creation by delegate of a custom config for OptionRoot called secondConfig
            services.Configure<OptionRoot>("secondConfig", myDelegatedOptions =>
            {
                myDelegatedOptions.Option1 = "delegation creation1";
                myDelegatedOptions.Option2 = "delegation creation2";
            });
            //First get the value from json
            services.Configure<OptionRoot>("thirdConfig", config);
            //After replace the value of Option2 with a delegated one.
            services.PostConfigure<OptionRoot>("thirdConfig", myPostDelegatedOptions =>
            {
                myPostDelegatedOptions.Option2 = "post delegation creation 2";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
