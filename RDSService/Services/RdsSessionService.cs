using System.Management.Automation;
using RDSServiceLibrary;
using RDSServiceLibrary.Helpers;
using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;
using Serilog;

namespace RDSService.Services
{
    public class RdsSessionService : IRdsSessionService
    {
        private static string GetConnectionBroker(string? connectionBroker) =>
            connectionBroker ?? DnsHelper.GetFqdn();

        public async Task<string> GetActiveManagementServer(string? connectionBroker = null)
        {
            connectionBroker = GetConnectionBroker(connectionBroker);
            var output = await ExecutePowerShellCommand("Get-RDConnectionBrokerHighAvailability",
                new { ConnectionBroker = connectionBroker });
            return output.FirstOrDefault()!.Properties["ActiveManagementServer"].Value.ToString()!;
        }

        public async Task<List<RdsSession>?> GetSessions(string? connectionBroker = null)
        {
            connectionBroker = GetConnectionBroker(connectionBroker);
            var output = await ExecutePowerShellCommand("Get-RDUserSession",
                new { ConnectionBroker = await GetActiveManagementServer(connectionBroker) });
            return output.Select(psObject => new RdsSession(psObject)).ToList();
        }

        public async Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null)
        {
            connectionBroker = GetConnectionBroker(connectionBroker);
            await ExecutePowerShellCommand("Disconnect-RDUser",
                new
                {
                    sessionInfo.HostServer, sessionInfo.UnifiedSessionId, Force = true,
                    ConnectionBroker = await GetActiveManagementServer(connectionBroker)
                });
            return true;
        }

        public async Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null)
        {
            connectionBroker = GetConnectionBroker(connectionBroker);
            await ExecutePowerShellCommand("Invoke-RDUserLogoff",
                new
                {
                    sessionInfo.HostServer, sessionInfo.UnifiedSessionId, Force = true,
                    ConnectionBroker = await GetActiveManagementServer(connectionBroker)
                });
            return true;
        }

        private async Task<PSDataCollection<PSObject>> ExecutePowerShellCommand(string command, object parameters)
        {
            Log.Information("Executing {Command} with {Parameters}", command, parameters);
            var ps = PowerShell.Create();
            await AddRemoteDesktopModule(ps);
            ps.AddCommand(command);
            foreach (var prop in parameters.GetType().GetProperties())
            {
                ps.AddParameter(prop.Name, prop.GetValue(parameters));
            }
            var output = await ps.InvokeAsync();
            if (!ps.HadErrors)
            {
                if (output.Count != 0)
                    Log.Information("Successfully Executed {Command} with {Parameters} and returned {OutputCount} results", command,
                        parameters, output.Count);
                else
                    Log.Information("Successfully Executed {Command} with {Parameters} and returned no results", command,
                        parameters);
                return output;
            }

            var errors = ps.Streams.Error.ReadAll();
            Log.Error("Error executing {Command}: {Errors}", command, errors);
            throw new RdsServiceException("Error executing PowerShell command.", errors, command, parameters);
        }

        private static async Task AddRemoteDesktopModule(PowerShell ps)
        {
            ps.AddCommand("Set-ExecutionPolicy").AddParameter("ExecutionPolicy", "RemoteSigned")
                .AddParameter("Scope", "Process").AddParameter("Force");
            await ps.InvokeAsync();
            ps.Commands.Clear();

            ps.AddCommand("Import-Module").AddParameter("Name", "RemoteDesktop").AddParameter("Verbose");
            await ps.InvokeAsync();
            if (ps.HadErrors)
            {
                var errors = ps.Streams.Error.ReadAll();
                Log.Error("Error importing RemoteDesktop module: {Errors}", errors);
                throw new RdsServiceException("Error importing RemoteDesktop module", errors, "Import-Module",
                    "RemoteDesktop");
            }

            ps.Commands.Clear();
        }
    }
}