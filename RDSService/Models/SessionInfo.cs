using System.Text.Json.Serialization;

namespace RDSService.Models;

public class SessionInfo
{
    public SessionInfo(string unifiedSessionId, string hostServer)
    {
        UnifiedSessionId = unifiedSessionId;
        HostServer = hostServer;
    }

    [JsonPropertyName("unifiedSessionId")]
    public string UnifiedSessionId { get; set; }

    [JsonPropertyName("hostServer")]
    public string HostServer { get; set; }
}
