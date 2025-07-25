using System.IO.Pipelines;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace HostMonitor;

public class PingHostPinger : IHostPinger
{
    public async Task<PingResult> PingAsync(string host, int port = 0, CancellationToken cancellationToken = default)
    {
        var result = new PingResult { Host = host };
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(host, 1000); //This is a 1s timeout

            result.Success = reply.Status == IPStatus.Success;
            result.RoundtripTime = reply.RoundtripTime;
            result.ErrorMessage = reply.Status.ToString();
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.ErrorMessage = $"ICMP: {ex.Message}";
        }

        //Check the port because why not
        if (port > 0)
        {
            try
            {
                using var tcpClient = new TcpClient();
                var connectTask = tcpClient.ConnectAsync(host, port);
                var timeoutTask = Task.Delay(1000, cancellationToken);

                var completed = await Task.WhenAny(connectTask, timeoutTask);
                result.TcpPortOpen = completed == connectTask && tcpClient.Connected;
            }
            catch
            {
                result.TcpPortOpen = false;
            }
        }

        return result;
    }
}