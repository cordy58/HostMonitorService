using Microsoft.Extensions.Options;

namespace HostMonitor;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITrackingRegistry _registry;
    private readonly IHostPinger _pinger;
    private readonly IMetricsStore _metricsStore;
    public Worker(
        ILogger<Worker> logger,
        ITrackingRegistry registry,
        IHostPinger pinger,
        IMetricsStore metricsStore)
    {
        _logger = logger;
        _registry = registry;
        _pinger = pinger;
        _metricsStore = metricsStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var trackedHosts = _registry.GetAll();

            foreach (var tracked in trackedHosts)
            {
                if ((now - tracked.LastPinged).TotalSeconds < tracked.IntervalSeconds)
                    continue;

                
                var result = await _pinger.PingAsync(tracked.Host, tracked.Port, stoppingToken);
                _metricsStore.AddPingResult(tracked.Host, tracked.Port, result);
                _registry.UpdateLastPinged(tracked.Host, tracked.Port, now);

                if (result.Success)
                {
                    _logger.LogInformation("Ping to {host} succeeded: RTT={rtt}ms, TCP port {port} open={tcp}",
                        tracked.Host, result.RoundtripTime, tracked.Port, result.TcpPortOpen);
                }
                else
                {
                    _logger.LogWarning("Ping to {host} failed (ICMP): {error}. TCP port {port} open={tcp}",
                        tracked.Host, result.ErrorMessage, tracked.Port, result.TcpPortOpen);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
