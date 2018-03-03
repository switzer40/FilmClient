using FilmAPI.Common.Services;
using FilmClient.Pages.Film;
using FilmClient.Pages.FilmPerson;
using FilmClient.Pages.Medium;
using FilmClient.Pages.Person;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmClient
{
    public class DevelopmentStartup
    {
        public DevelopmentStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFilmService, FilmMockService>();
            services.AddScoped<IPersonService, PersonMockService>();
            services.AddScoped<IMediumService, MediumMockService>();
            services.AddScoped<IFilmPersonService, FilmPersonMockService>();
            return ConfigureIocServices(services);
        }

        private IServiceProvider ConfigureIocServices(IServiceCollection services)
        {
            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(ProductionStartup));
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

            app.UseMvc();
        }
    }
}
               
      
