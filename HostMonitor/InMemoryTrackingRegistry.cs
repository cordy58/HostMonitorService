namespace HostMonitor;

public class InMemoryTrackingRegistry : ITrackingRegistry
{
    private readonly List<TrackedHost> _hosts = new();
    private readonly object _lock = new();

    public void AddHost(string host, int port, int intervalSeconds)
    {
        lock (_lock)
        {
            if (!_hosts.Any(h => h.Host == host && h.Port == port))
            {
                _hosts.Add(new TrackedHost
                {
                    Host = host,
                    Port = port,
                    IntervalSeconds = intervalSeconds
                });
            }
        }
    }

    public void RemoveHost(string host, int port)
    {
        lock (_lock)
        {
            _hosts.RemoveAll(h => h.Host == host && h.Port == port);
        }
    }

    public List<TrackedHost> GetAll()
    {
        lock (_lock)
        {
            return _hosts.Select(h => new TrackedHost
            {
                Host = h.Host,
                Port = h.Port,
                IntervalSeconds = h.IntervalSeconds,
                LastPinged = h.LastPinged
            }).ToList(); // Return a copy to avoid mutation
        }
    }

    // Optional: internal method for Worker to mark a host as recently pinged
    public void UpdateLastPinged(string host, int port, DateTime time)
    {
        lock (_lock)
        {
            var match = _hosts.FirstOrDefault(h => h.Host == host && h.Port == port);
            if (match != null)
            {
                match.LastPinged = time;
            }
        }
    }
}
