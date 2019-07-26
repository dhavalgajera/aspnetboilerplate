﻿using System;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.AspNetCore.TestBase;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abp.AspNetCore.App
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // TODO@3.0 Should we remove EnableEndpointRouting = false after ASP.NET Core 3.0 release ?
            var mvc = services.AddMvc().AddXmlSerializerFormatters();
            
            mvc.PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AbpAspNetCoreModule).GetAssembly()));

            //Configure Abp and Dependency Injection
            return services.AddAbp<AppModule>(options =>
            {
                //Test setup
                options.SetupTest();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                app.ApplicationServices.GetRequiredService<IAbpAspNetCoreConfiguration>().EndpointConfiguration
                    .ConfigureAllEndpoints(endpoints);
            });

            app.UseEndpoints(endpoints =>
            {
                app.ApplicationServices.GetRequiredService<IAbpAspNetCoreConfiguration>().EndpointConfiguration.ConfigureAllEndpoints(endpoints);
            });
        }
    }
}
