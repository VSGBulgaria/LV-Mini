using AuthorizationServer.Configuration;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using static AuthorizationServer.Configuration.InMemoryConfiguration;

namespace AuthorizationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            string connectionString = Configuration.GetConnectionString("AuthrizationServer");
            var assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(assembly));
                });

            services.AddDbContext<LvMiniDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LV_MiniDatabase")));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserValidator, UserValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MigrateInMemoryDataToSqlServer(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


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

                foreach (var apiResource in ApiResources())
                {
                    if (!context.ApiResources.Any(api => api.Name == apiResource.Name))
                    {
                        context.ApiResources.Add(apiResource.ToEntity());
                    }
                }

                foreach (var identityResource in IdentityResources())
                {
                    if (!context.IdentityResources.Any(identity => identity.Name == identityResource.Name))
                    {
                        context.IdentityResources.Add(identityResource.ToEntity());
                    }
                }

                foreach (var client in Clients())
                {
                    if (!context.Clients.Any(opt => opt.ClientId == client.ClientId))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    if (context.Clients.Any())
                    {
                        var entity = context.Clients.Single(x => x.ClientId == client.ClientId);
                        if (entity != null)
                        {
                            var clientEntity = client.ToEntity();
                            entity.AllowedScopes = clientEntity.AllowedScopes;
                            context.Clients.Update(entity);
                        }
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
