using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using RDSService;
using RDSService.Interfaces;
using RDSService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add configuration based on environment
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true,
    reloadOnChange: true);

// Configure Logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddScoped<IRdsSessionService, RdsSessionService>();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "RDS Service",
            Version = "v1",
            Description = "Service for managing RDS sessions",
            Contact = new OpenApiContact
            {
                Name = "Royal United Mortgage IT",
                Email = "it@royalunited.com",
                Url = new Uri("https://www.royalunitedmortgage.com")
            }
        });
    });
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("MyCorsPolicy",
            builder => { builder.WithOrigins("http://localhost:8080").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
    });
    
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    
    app.UseCors("MyCorsPolicy");
    app.UseAuthorization();
    app.UseAuthentication();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RDS Service V1"); });

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed to start");
    throw;
}