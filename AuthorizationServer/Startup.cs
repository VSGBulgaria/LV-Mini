using AuthorizationServer.Configuration;
using Data.Service.Core;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Reflection;
using static AuthorizationServer.Configuration.InMemoryConfiguration;

namespace AuthorizationServer
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("AuthrizationServer"),
                            options => options.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseSqlServer(Configuration.GetConnectionString("AuthrizationServer"),
                            options => options.MigrationsAssembly(assembly));
                });
            services.AddDbContext<LvMiniDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LV_MiniDatabase")));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserValidator, UserValidator>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MigrateInMemoryDataToSqlServer(app);

            _loggerFactory.AddConsole();
            _loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }

        public void MigrateInMemoryDataToSqlServer(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (Client client in Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.IdentityResources.Any())
                {
                    foreach (IdentityResource identityResource in IdentityResources())
                    {
                        context.IdentityResources.Add(identityResource.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any())
                {
                    foreach (ApiResource apiResource in ApiResources())
                    {
                        context.ApiResources.Add(apiResource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
