using AutoMapper;
using Data.Service.Persistance;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace LVMini
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
            services.AddAutoMapper();
            services.AddDbContext<LvMiniDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LV_MiniDatabase")));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(opt =>
                {
                    opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(opt =>
                {
                    opt.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    opt.Authority = "http://localhost:55817/";
                    opt.RequireHttpsMetadata = false;
                    opt.ClientId = "lvmini_code";
                    opt.ClientSecret = "interns";
                    opt.ResponseType = "code id_token";
                    opt.Scope.Add("lvminiAPI");
                    opt.Scope.Add("lvmini_admin");
                    opt.Scope.Add("roles");
                    opt.Scope.Add("offline_access");
                    opt.SaveTokens = true;
                    opt.GetClaimsFromUserInfoEndpoint = true;
                });

            services.AddMvc();
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
