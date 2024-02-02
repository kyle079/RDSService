using System.Net;
using System.Net.NetworkInformation;

namespace RDSServiceLibrary.Helpers;

public static class DnsHelper
{
    public static string GetFqdn()
    {
        var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
        var hostName = Dns.GetHostName();

        if (!hostName.EndsWith(domainName))  // if hostname does not already include domain name
        {
            hostName += "." + domainName;   // append the domain name part
        }

        return hostName;
    }
}