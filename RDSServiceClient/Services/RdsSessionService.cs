using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;

namespace RDSServiceClient.Services;
public class RdsSessionService : IRdsSessionService
{
    public async Task<string> GetActiveManagementServer(string? connectionBroker = null)
    {
        throw new NotImplementedException();
    }

    public async Task<List<RdsSession>> GetSessions(string? connectionBroker = null)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null)
    {
        throw new NotImplementedException();
    }
}