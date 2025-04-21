using Auth.Repositories;
using Auth.Repositories.Interfaces;
using Auth.Services;
using Auth.Services.Interfaces;
namespace Auth.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection InjectServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserDataChangeRequestService, UserDataChangeRequestService>();
            services.AddScoped<IUserDataChangeRequestRepository, UserDataChangeRequestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            

            return services;
        }
    }
}
