using System.Management.Automation;

namespace RDSService.Models;

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

    public string CollectionName { get; set; }
    public string DomainName { get; set; }
    public string UserName { get; set; }
    public string HostServer { get; set; }
    public string UnifiedSessionId { get; set; }
}