using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Helpers;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationHelper = new ConfigurationHelper(configuration);
        string[] origins = configurationHelper.GetStringArray("CorsOrigins");

        services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              b =>
                              {
                                  b.WithOrigins(origins)
                                    .AllowAnyHeader()
                                    .AllowAnyMethod();
                              });
        });

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

        services.AddHttpContextAccessor();

        return services;
    }

    public static IServiceCollection AddMyAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
            {
                c.Authority = $"https://{configuration["Auth0:Domain"]}";
                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = configuration["Auth0:Audience"],
                    ValidIssuer = $"{configuration["Auth0:Domain"]}"
                };
                c.SecurityTokenValidators.Add(new CustomJwtSecurityTokenValidator(configuration["DemoToken"]));
            });

        return services;
    }

    public static void UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config.WriteTo.Console().MinimumLevel.Warning();

            var conStrBuilder = new NpgsqlConnectionStringBuilder(
                    context.Configuration.GetConnectionString("DefaultConnection"));

            var password = context.Configuration["DB_PASSWORD"];
            if (string.IsNullOrWhiteSpace(password))
            {
                password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            }
            conStrBuilder.Password = password;

            var connectionString = conStrBuilder.ConnectionString;
            config.WriteTo.PostgreSQL(connectionString, "logs", needAutoCreateTable: true)
                .MinimumLevel.Warning();
        });
    }

    public static void UseCorsPolicy(this WebApplication app)
    {
        app.UseCors(MyAllowSpecificOrigins);
    }
}
