using Arabamcom2.DbContext;
using Arabamcom2.IService;
using Arabamcom2.Service;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;


var logger = LogManager
    .Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddSingleton<ConnectionHelper>();
    builder.Services.AddControllers();

    builder.Logging.ClearProviders();

    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);

    builder.Host.UseNLog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //builder.Services.AddHealthChecks();
    //builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
    //builder.Services.AddHealthChecksUI(settings =>
    //    settings.AddHealthCheckEndpoint("Service 1", "https://localhost:44383/health")).AddInMemoryStorage();

    builder.Services.AddHealthChecks()
        .AddCheck("arabam-api", () => HealthCheckResult.Healthy("Live"), new[] { "ready" })
       .AddSqlServer(
           connectionString: builder.Configuration.GetConnectionString("DefaultConnectionString"),
           healthQuery: "SELECT 1;",
           name: "SQL",
           failureStatus: HealthStatus.Unhealthy | HealthStatus.Degraded,
           tags: new string[] { "SQL" ,"Db"},
           timeout: TimeSpan.FromSeconds(30))
        .Services.AddHealthChecksUI()
        .AddInMemoryStorage();

    builder.Services.AddScoped<IAdvertService, AdvertService>();
    builder.Services.AddHttpContextAccessor();
    var app = builder.Build();
    app.UseStaticFiles();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    



    app.MapHealthChecks("/health/detail", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status200OK
                    }
    });

    app.MapHealthChecks("/health/ready", new HealthCheckOptions
    {
        Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

    app.MapHealthChecksUI(opt =>
    {
        opt.UIPath = "/health-ui";
    });




    app.Run();


}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}

finally
{
    LogManager.Shutdown();
}