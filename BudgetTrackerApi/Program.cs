using Application;
using Application.Common.Interfaces;
using BudgetTrackerApi.Filters;
using BudgetTrackerApi.Services;
using FluentValidation.AspNetCore;
using Helpers;
using Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using Serilog;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

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

var configurationHelper = new ConfigurationHelper(builder.Configuration);
string[] origins = configurationHelper.GetStringArray("CorsOrigins");

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      b =>
                      {
                          b.WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);


builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>())
        .AddFluentValidation(x => x.AutomaticValidationEnabled = false); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMyAuthentication(builder.Configuration);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
//     {
//         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
//         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//         {
//             ValidAudience = builder.Configuration["Auth0:Audience"],
//             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
//         };
//     });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("general:read-write", p => p.
        RequireAuthenticatedUser().
        RequireClaim("scope", "general:read-write"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Initialise database
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetService<IApplicationDbContext>();
        if (context is not null)
        {
            await context.MigrateAsync();
        }
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
