using System.Management.Automation;
using System.Text.Json.Serialization;

namespace RDSServiceLibrary.Models;

public class RdsSession
{
    public RdsSession(PSObject psObject)
    {
        CollectionName = psObject.Properties["CollectionName"].Value.ToString() ?? string.Empty;
        DomainName = psObject.Properties["DomainName"].Value.ToString() ?? string.Empty;
        UserName = psObject.Properties["UserName"].Value.ToString() ?? string.Empty;
        HostServer = psObject.Properties["HostServer"].Value.ToString() ?? string.Empty;
        UnifiedSessionId = psObject.Properties["UnifiedSessionId"].Value.ToString() ?? string.Empty;
    }

    [JsonPropertyName("collectionName")]
    public string CollectionName { get; set; }
    [JsonPropertyName("domainName")]
    public string DomainName { get; set; }
    [JsonPropertyName("userName")]
    public string UserName { get; set; }
    [JsonPropertyName("hostServer")]
    public string HostServer { get; set; }
    [JsonPropertyName("unifiedSessionId")]
    public string UnifiedSessionId { get; set; }
}