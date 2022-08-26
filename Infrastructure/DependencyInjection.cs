using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conStrBuilder = new NpgsqlConnectionStringBuilder(
            configuration.GetConnectionString("DefaultConnection"));

        var password = configuration["DB_PASSWORD"];
        if (string.IsNullOrWhiteSpace(password))
        {
            password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        }

        conStrBuilder.Password = password;

        var connectionString = conStrBuilder.ConnectionString;

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
            options.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>()));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ISaveChangesInterceptor, DefaultSavingChangesInterceptor>();

        return services;
    }
}
