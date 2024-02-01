using System.Collections.ObjectModel;
using System.Management.Automation;
using RDSService.Interfaces;
using RDSService.Models;
using Serilog;

namespace RDSService.Services;

public class RdsSessionService : IRdsSessionService
{
    public async Task<string> GetActiveManagementServer()
    {
        try
        {
            var ps = PowerShell.Create()
                .AddCommand("Get-RDConnectionBrokerHighAvailability")
                .AddParameter("ConnectionBroker", "rdscb01.royal.corp");
            var output = await ps.InvokeAsync();
            if (ps.HadErrors)
            {
                var errors = ps.Streams.Error.ReadAll() ?? new Collection<ErrorRecord>();
                Log.Error("Error getting active management server: {Errors}", errors);
                throw new Exception(
                    $"Error getting active management server {errors.Count} found in Powershell script");
            }

            if (output.Count == 0 || output.FirstOrDefault()?.ToString() == "")
            {
                throw new Exception("No active management server found");
            }

            return output.FirstOrDefault()?.Properties["ActiveManagementServer"].Value.ToString() ??
                   throw new Exception("No active management server found");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting active management server");
            throw;
        }
    }

    public async Task<List<RdsSession>> GetSessions()
    {
        try
        {
            var ps = PowerShell.Create()
                .AddCommand("Get-RDUserSession")
                .AddParameter("ConnectionBroker", await GetActiveManagementServer());
            var output = await ps.InvokeAsync();
            if (ps.HadErrors)
            {
                var errors = ps.Streams.Error.ReadAll();
                Log.Error("Error getting RDS sessions: {Errors}", errors);
                throw new Exception("Error getting RDS sessions. Powershell script had errors.");
            }
            if (output.Count == 0)
            {
                throw new Exception("No RDS sessions found");
            }
            var rdsSessions = output.Select(psObject => new RdsSession(psObject)).ToList();
            return rdsSessions;
        }
        catch (Exception e)
        {
            Log.Error(e, "Error getting RDS sessions");
            throw;
        }
    }

    public async Task<bool> DisconnectSession(SessionInfo sessionInfo)
    {
        try
        {
            var ps = PowerShell.Create()
                .AddCommand("Disconnect-RDUser")
                .AddParameter("HostServer", sessionInfo.HostServer)
                .AddParameter("UnifiedSessionID", sessionInfo.UnifiedSessionId)
                .AddParameter("Force");
            await ps.InvokeAsync();
            if (!ps.HadErrors) return true;
            var errors = ps.Streams.Error.ReadAll();
            Log.Error("Error disconnecting RDS session: {Errors}", errors);
            throw new Exception("Error disconnecting RDS session");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error disconnecting RDS session");
            throw;
        }
    }

    public async Task<bool> LogOffSession(SessionInfo sessionInfo)
    {
        try
        {
            var ps = PowerShell.Create()
                .AddCommand("Import-Module RemoteDesktop")
                .AddCommand("Invoke-RDUserLogoff")
                .AddParameter("HostServer", sessionInfo.HostServer)
                .AddParameter("UnifiedSessionID", sessionInfo.UnifiedSessionId)
                .AddParameter("Force");
            await ps.InvokeAsync();
            if (!ps.HadErrors) return true;
            var errors = ps.Streams.Error.ReadAll();
            Log.Error("Error logging off RDS session: {Errors}", errors);
            throw new Exception("Error logging off RDS session");
        }
        catch (Exception e)
        {
            Log.Error(e, "Error logging off RDS session");
            throw;
        }
    }
}