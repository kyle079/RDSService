using Microsoft.OpenApi.Models;
using RDSService.Services;
using RDSServiceLibrary.Interfaces;
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
    Log.Information("Application is starting up...");
    Log.Information("Environment: {Environment}", builder.Environment.EnvironmentName);

    // Add services to the container.
    Log.Information("Adding services to the container...");
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
            builder =>
            {
                builder.WithOrigins("http://localhost:8080").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
    });

    Log.Information("Building application...");
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    Log.Information("Configuring HTTP request pipeline...");
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    Log.Information("Adding middleware...");
    app.UseCors("MyCorsPolicy");
    app.UseAuthorization();
    app.UseAuthentication();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "RDS Service V1"); });

    Log.Information("Adding endpoints...");
    app.MapControllers();

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application failed to start");
    throw;
}