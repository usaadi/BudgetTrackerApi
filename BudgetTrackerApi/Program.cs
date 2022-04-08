using Infrastructure;
using Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Common.Interfaces;
using BudgetTrackerApi.Services;
using BudgetTrackerApi.Filters;
using FluentValidation.AspNetCore;
using Serilog;
using Npgsql;
using Microsoft.AspNetCore.HttpOverrides;

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

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      b =>
                      {
                          b.WithOrigins("https://localhost:3000",
                                        "https://budget-tracker.nfshost.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>())
        .AddFluentValidation(x => x.AutomaticValidationEnabled = false); ;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
         };
     });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("general:read-write", p => p.
        RequireAuthenticatedUser().
        RequireClaim("scope", "general:read-write"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

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
