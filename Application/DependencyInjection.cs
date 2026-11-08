using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Fcg.Application.Users;

namespace Fcg.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();
        services.AddScoped<AuthService>();
        services.AddScoped<UserService>();
        return services;
    }
}
