using Application.Common.Interfaces;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

builder.Services.AddMyAuthentication(builder.Configuration);
builder.Services.AddMyAuthorization(builder.Configuration);

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
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
});

//app.UseHttpsRedirection();

app.UseCorsPolicy();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
