﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Bundler.Transformers;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Bundler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("da")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });


            app.UseBundles(options =>
                {
                    options.Enabled = true;// !env.IsDevelopment();

                    options.AddJs("/all.js", new[] { "js/site.js", "js/b.js" })
                           .Localize<BundlerSample.Strings>(app);

                    options.AddCss("/all.css", "css/site.css", "lib/bootstrap/dist/css/bootstrap.css");
                });

            app.MinifyJavaScript();
            app.MinifyCss();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

}

namespace BundlerSample
{
    public class Strings
    {

    }
}
