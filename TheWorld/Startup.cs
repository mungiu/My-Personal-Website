using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;
        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();
        }


        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        // For more information on how to configure your application, 
        // visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development") || 
                _env.IsEnvironment("Testing"))
            {
                //creates service instance when required
                services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                //TODO Implement real Mail Service
            }

            //registering services 
            //which will now be injectible in different parts of project
            services.AddDbContext<WorldContext>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddTransient<GeoCoordsService>();
            services.AddTransient<WorldContextSeedData>();
            services.AddLogging();
            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    //making sure properties are cammel cased
                    config.SerializerSettings.ContractResolver = 
                    new CamelCasePropertyNamesContractResolver();
                });
        }


        // This method gets called by the runtime. 
        // Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            WorldContextSeedData seeder,
            ILoggerFactory factory)
        {
            //configuring maps (bidirectional mapping)
            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                //logging error info to debug streem
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                //logging error messages
                factory.AddDebug(LogLevel.Error);
            }

            //serving the request
            app.UseStaticFiles();
            //the router
            app.UseMvc(config =>
            {
            config.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}", //id is now optional
                defaults: new { controller = "App", action = "Index" }
                );
            });
            //calling a synchronious operaton (cannot be asynch)
            seeder.EnsureSeedData().Wait();
        }
    }
}
