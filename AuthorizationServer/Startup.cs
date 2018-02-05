using AuthorizationServer.Services;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthorizationServer
{
    public class Startup
    {
        public static IConfigurationRoot ConfigurationRoot;
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            ConfigurationRoot = builder.Build();

            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string authServerConnectionString = Configuration["connectionStrings:AuthServer"];
            string userDbConnectionString = Configuration["connectionStrings:LVMiniDatabase"];
            string assembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<LvMiniDbContext>(o => o.UseSqlServer(userDbConnectionString));
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMvc();

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.AuthenticationDisplayName = "Windows";
            });

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authServerConnectionString,
                            sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseSqlServer(authServerConnectionString,
                            sql => sql.MigrationsAssembly(assembly));
                })
                .AddDeveloperSigningCredential()
                .AddLvMiniUserStore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LvMiniDbContext lvMiniDbContext,
            ConfigurationDbContext configurationDbContext, PersistedGrantDbContext persistedGrantDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            configurationDbContext.Database.Migrate();
            configurationDbContext.SeedDataForContext();

            persistedGrantDbContext.Database.Migrate();

            lvMiniDbContext.Database.Migrate();
            lvMiniDbContext.SeedDataForContext();


            FileExtensionContentTypeProvider typeProvider = new FileExtensionContentTypeProvider();
            if (!typeProvider.Mappings.ContainsKey(".woff2"))
            {
                typeProvider.Mappings.Add(".woff2", "application/font-woff2");
            }
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
