namespace HostMonitor;

public interface ITrackingRegistry
{
    void AddHost(string host, int port, int intervalSeconds);
    void RemoveHost(string host, int port);
    List<TrackedHost> GetAll();
    void UpdateLastPinged(string host, int port, DateTime dateTime);
}
