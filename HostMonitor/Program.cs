using HostMonitor;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


var builder = WebApplication.CreateBuilder(args);

const string AllowFrontendPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontendPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.Configure<MonitorOptions>(builder.Configuration);
builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<IHostPinger, PingHostPinger>();
builder.Services.AddSingleton<IMetricsStore, MetricsStore>();
builder.Services.AddSingleton<ITrackingRegistry, InMemoryTrackingRegistry>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors(AllowFrontendPolicy);

using (var scope = app.Services.CreateScope())
{
    var options = scope.ServiceProvider.GetRequiredService<IOptions<MonitorOptions>>().Value;
    var registry = scope.ServiceProvider.GetRequiredService<ITrackingRegistry>();

    foreach (var hostEntry in options.Hosts.Split(',', StringSplitOptions.RemoveEmptyEntries))
    {
        registry.AddHost(hostEntry.Trim(), options.Port, options.Interval);
    }
}

app.MapGet("/api/pings", (IMetricsStore store) =>
{
    var pings = store.GetAll()
        .Select(p => new {
            p.Host,
            p.Success,
            p.RoundtripTime,
            p.TcpPortOpen,
            p.ErrorMessage,
            CheckedAt = p.CheckedAt.ToString("o") // Date was being weird so I did this
        });

    return Results.Ok(pings);
});

app.Run();
