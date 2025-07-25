namespace HostMonitor;

public class MonitorOptions
{
    public string Hosts { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
    public int Interval { get; set; } = 5; // 5 second default

}