using System.Net.Http.Json;
using RDSServiceLibrary;
using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;

namespace RDSServiceClient
{
    public class RdsSessionService : IRdsSessionService
    {
        private readonly HttpClient _httpClient;

        public RdsSessionService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public Task<string> GetActiveManagementServer(string? connectionBroker = null) =>
            ProcessRequest<string>("GetActiveManagementServer");

        public Task<List<RdsSession>> GetSessions(string? connectionBroker = null) =>
            ProcessRequest<List<RdsSession>>("GetSessions");

        public Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest("DisconnectSession", sessionInfo);

        public Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest("LogOffSession", sessionInfo);

        private async Task<T> ProcessRequest<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            await HandleError(response);
            return await response.Content.ReadFromJsonAsync<T>() ??
                   throw new RdsServiceException("An error occurred processing the response from the RDS service");
        }

        private async Task<bool> ProcessPostRequest(string url, SessionInfo sessionInfo)
        {
            var response = await _httpClient.PostAsJsonAsync(url, sessionInfo);
            await HandleError(response);
            return true;
        }

        private static async Task HandleError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<RdsServiceException>();
                if (result != null) throw result;
                throw new RdsServiceException("An error occurred calling the RDS service");
            }
        }
    }
}