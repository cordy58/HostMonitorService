namespace HostMonitor;

public class PingResult
{
    public string Host { get; set; } = string.Empty;
    public bool Success { get; set; }
    public long RoundtripTime { get; set; } = -1; //This is in ms
    public bool TcpPortOpen { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}