﻿using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LVMini
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAutoMapper();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opttions =>
                {
                    opttions.ExpireTimeSpan = TimeSpan.FromMinutes(60);

                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, "OpenID Connect", opt =>
                 {
                     opt.Authority = "http://localhost:55817/";
                     opt.RequireHttpsMetadata = false;

                     opt.ClientId = "lvmini_code";
                     opt.ClientSecret = "interns";
                     opt.SignedOutRedirectUri = new PathString("/Accounts/Login");
                     opt.ResponseType = "code id_token";

                     opt.Scope.Clear();
                     opt.Scope.Add("openid");
                     opt.Scope.Add("profile");
                     opt.Scope.Add("lvminiAPI");
                     opt.Scope.Add("lvmini_admin");
                     opt.Scope.Add("offline_access");

                     opt.GetClaimsFromUserInfoEndpoint = true;
                     opt.SaveTokens = true;

                     opt.TokenValidationParameters = new TokenValidationParameters
                     {
                         NameClaimType = JwtClaimTypes.Name,
                         RoleClaimType = JwtClaimTypes.Role
                     };
                     opt.Events = new OpenIdConnectEvents()
                     {
                         OnTokenValidated = ctx =>
                         {
                             var identity = ctx.Principal.Identity as ClaimsIdentity;
                             var subjectClaim = identity.Claims.FirstOrDefault(z => z.Type == "sub");

                             var newClaimsIdentity = new ClaimsIdentity(ctx.Principal.Identity.AuthenticationType, "given_name", "role");
                             newClaimsIdentity.AddClaim(subjectClaim);

                             ctx.Principal = new ClaimsPrincipal(newClaimsIdentity);

                             return Task.FromResult(0);
                         }
                     };
                 });
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

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

