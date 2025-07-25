using System.Collections.Concurrent;

namespace HostMonitor;

public class MetricsStore : IMetricsStore
{
    private readonly ConcurrentDictionary<string, List<PingResult>> _pingHistory = new();
    private readonly object _lock = new();

    private static string GetKey(string host, int port) => $"{host}:{port}";

    public void AddPingResult(string host, int port, PingResult result)
    {
        string key = GetKey(host, port);

        lock (_lock)
        {
            if (!_pingHistory.ContainsKey(key))
                _pingHistory[key] = new List<PingResult>();

            _pingHistory[key].Add(result);
        }
    }

    public IEnumerable<PingResult> GetAll()
    {
        lock (_lock)
        {
            return _pingHistory.Values.SelectMany(list => list).ToList();
        }
    }

    public IEnumerable<PingResult> GetForHost(string host, int port)
    {
        string key = GetKey(host, port);

        lock (_lock)
        {
            return _pingHistory.TryGetValue(key, out var list)
                ? new List<PingResult>(list)
                : Enumerable.Empty<PingResult>();
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _pingHistory.Clear();
        }
    }
}
