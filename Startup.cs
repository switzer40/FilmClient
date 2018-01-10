using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FilmClient.Pages.Film;
using FilmClient.Pages.Person;
using FilmClient.Pages.Medium;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Shared;
using Microsoft.Extensions.Logging;
using StructureMap;
using FilmAPI.Common.Services;

namespace FilmClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            
        }

        public IConfiguration Configuration { get; }
       
        public IServiceProvider ConfigureDevelopmentServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IFilmService, FilmMockService>();
            services.AddSingleton<IPersonService, PersonMockService>();
            services.AddSingleton<IMediumService, MediumMockService>();
            services.AddSingleton<IFilmPersonService, FilmPersonMockService>();
           return ConfigureServices(services);
        }
        public IServiceProvider ConfigureProductionServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IFilmService, FilmAPIService>();
            services.AddScoped<IPersonService, PersonAPIService>();
            services.AddScoped<IMediumService, MediumAPIService>();
            services.AddScoped<IFilmPersonService, FilmPersonAPIService>();
            return ConfigureServices(services);
        }
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return ConfigureIocServices(services);       
        }

        private IServiceProvider ConfigureIocServices(IServiceCollection services)
        {
            var container = new Container();
            container.Configure(config =>
            {
            config.Scan(_ =>
            {
                _.AssemblyContainingType(typeof(Startup));
                _.AssemblyContainingType(typeof(KeyService));
                _.WithDefaultConventions();
            });
                config.For(typeof(IFilmService)).Add(typeof(FilmAPIService));
                config.For(typeof(IFilmService)).Add(typeof(FilmMockService));
                config.For(typeof(IFilmPersonService)).Add(typeof(FilmPersonAPIService));
                config.For(typeof(IFilmPersonService)).Add(typeof(FilmPersonMockService));
                config.For(typeof(IMediumService)).Add(typeof(MediumAPIService));
                config.For(typeof(IMediumService)).Add(typeof(MediumMockService));
                config.For(typeof(IPersonService)).Add(typeof(PersonAPIService));
                config.For(typeof(IPersonService)).Add(typeof(PersonMockService));
                config.Populate(services);
            });
            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
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

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
