using AutoMapper;
using Data.Service.Core;
using Data.Service.Persistance;
using Data.Service.Persistance.Repositories;
using LVMiniApi.Facebook;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LVMiniApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string UserInformationEndpoint { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddDbContext<LvMiniDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LV_MiniDatabase")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher<IUser>, PasswordHasher<IUser>>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddFacebook(fb =>
                {
                    fb.AppId = "1778483512171539";
                    fb.AppSecret = "d3694477a966b346a9c85139aebcda8f";
                    fb.BackchannelHttpHandler = new FacebookBackChannelHandler();
                    UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,email";
                });
           
        
            services.AddMvc(opt =>
            {
                if (!_env.IsProduction())
                {
                    opt.SslPort = 44300;
                }
                opt.Filters.Add(new RequireHttpsAttribute());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
