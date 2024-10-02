using System;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Interfaces;
using WebApi.Services;

namespace WebApi.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration config
    )
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"))
        );
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        // services.AddTransient<ITokenService, TokenService>();
        // services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}
