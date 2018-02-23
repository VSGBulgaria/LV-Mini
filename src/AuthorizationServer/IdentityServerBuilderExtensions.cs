using AuthorizationServer.Services;
using Data.Service.Core.Interfaces;
using Data.Service.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationServer
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddLvMiniUserStore(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.AddProfileService<UserProfileService>();
            return builder;
        }
    }
}
