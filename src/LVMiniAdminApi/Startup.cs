using AutoMapper;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using LVMiniAdminApi.Contracts;
using LVMiniAdminApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace LVMiniAdminApi
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
            services.AddDbContext<LvMiniDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LV_MiniDatabase")));

            services.AddAutoMapper();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }); ;

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IModifiedUserHandler, ModifiedUserHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Admin API", Version = "1.0", Description = "Admin API Consumed by LVMini project." });

            });

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:55817/";
                    options.ApiName = "lvmini_admin";
                    options.ApiSecret = "adminAPIsecret";
                });
            services.AddAuthorization(options =>
                {
                    options.AddPolicy("AdminOnly", policy =>
                    {
                        policy.RequireClaim("role", "admin");
                    });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin API");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
