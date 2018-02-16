using AutoMapper;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;

namespace LVMiniApi
{
    internal class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                options.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "LvMiniAPI",
                    Version = "1.0",
                    Description = "The main API for the LvMini project. To use most of the API you have to be an authenticated user.",
                    TermsOfService = "None",
                    Contact = new Contact { Email = "simeon.banev3@gmail.com", Name = "Simeon Banev", Url = "https://github.com/simonbane" },
                });

                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "LVMiniApi.xml");
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Swagger", new OAuth2Scheme
                {
                    Flow = "implicit",
                    AuthorizationUrl = "http://localhost:55817/connect/authorize",
                    TokenUrl = "http://localhost:55817/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "lvminiAPI", "lvminiAPI" },
                        {"mainAPIsecret", "mainAPIsecret" }
                    }
                });
            });

            services.AddAutoMapper();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<LvMiniDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("LVMiniDatabase"));
                });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://localhost:55817/";

                    options.ApiName = "lvminiAPI";
                    options.ApiSecret = "mainAPIsecret";
                    options.SupportedTokens = SupportedTokens.Both;
                });
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
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault happened. Please try again later.");
                    });
                });
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<LvMiniDbContext>();
                context.Database.Migrate();
                context.SeedDataForContext();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LvMiniAPI 1.0");
                c.ConfigureOAuth2("api.name.swagger", "swagger", "swagger-ui-realm", "API Swagger UI");
            });
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
