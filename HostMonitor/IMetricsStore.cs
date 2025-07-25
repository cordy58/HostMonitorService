namespace HostMonitor;

public interface IMetricsStore
{
    void AddPingResult(string host, int port, PingResult result);
    IEnumerable<PingResult> GetAll();
    IEnumerable<PingResult> GetForHost(string host, int port);
    void Clear();
}
