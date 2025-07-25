namespace HostMonitor;

public class TrackedHost
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public int IntervalSeconds { get; set; }
    public DateTime LastPinged { get; set; } = DateTime.MinValue;
}
