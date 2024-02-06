#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace RDSServiceClient;

public class RdsServiceClientOptions
{
    public RdsServiceClientOptions() { }
    public RdsServiceClientOptions(string baseUrl,
        string disconnectUrl = "DisconnectSession",
        string logoffUrl = "LogOffSession",
        string sessionsUrl = "GetSessions",
        string activeManagementServerUrl = "GetActiveManagementServer")
    {
        BaseUrl = baseUrl;
        DisconnectUrl = disconnectUrl;
        LogoffUrl = logoffUrl;
        SessionsUrl = sessionsUrl;
        ActiveManagementServerUrl = activeManagementServerUrl;
    }

    public string BaseUrl { get; set; }
    public string DisconnectUrl { get; set; }
    public string LogoffUrl { get; set; }
    public string SessionsUrl { get; set; }
    public string ActiveManagementServerUrl { get; set; }
}