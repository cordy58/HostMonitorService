namespace HostMonitor;

public interface IHostPinger
{
    Task<PingResult> PingAsync(string host, int port = 0, CancellationToken cancellationToken = default);
}