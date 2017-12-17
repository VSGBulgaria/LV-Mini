using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Data.Service;
using Data.Service.Persistance;
using Data.Service.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

namespace LVMiniApi
{
    using AutoMapper;
    using Data.Service.Entities;
    using MappedModels;

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
            services.AddMvc();
            services.AddAutoMapper();
            this.ConfigureMapper();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        private void ConfigureMapper()
        {
            Mapper.Initialize(expression => {
                expression.CreateMap<User, BasicUser>();
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
