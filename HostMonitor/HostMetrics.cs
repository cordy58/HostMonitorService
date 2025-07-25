namespace HostMonitor;

public class HostMetrics
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 0;

    // Current Status
    public bool IsUp { get; set; }
    public bool LastIcmpSuccess { get; set; }
    public bool LastTcpOpen { get; set; }
    public long LastRoundtripTime { get; set; } = -1;
    public DateTime LastChecked { get; set; } = DateTime.MinValue;

    //Running Metrics
    public int TotalChecks { get; private set; } = 0;
    public int TotalSuccesses { get; private set; } = 0;
    public int ConsecutiveFailures { get; private set; } = 0;

    public double SuccessRate => TotalChecks == 0 ? 0 : (double)TotalSuccesses / TotalChecks;

    public void Update(PingResult result)
    {
        LastChecked = DateTime.UtcNow;
        LastIcmpSuccess = result.Success;
        LastTcpOpen = result.TcpPortOpen;
        LastRoundtripTime = result.RoundtripTime;

        bool up = result.Success || result.TcpPortOpen;
        IsUp = up;

        TotalChecks++;

        if (up)
        {
            TotalSuccesses++;
            ConsecutiveFailures = 0;
        }
        else
        {
            ConsecutiveFailures++;
        }
    }
}