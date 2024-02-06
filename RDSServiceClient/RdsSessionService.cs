using System.Net.Http.Json;
using RDSServiceLibrary;
using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;
using System.Text.Json;

namespace RDSServiceClient
{
    public class RdsSessionService : IRdsSessionService
    {
        private readonly HttpClient _httpClient;

        public RdsSessionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GetActiveManagementServer(string? connectionBroker = null) =>
            ProcessRequest<string>("RDS/GetActiveManagementServer");

        public Task<List<RdsSession>> GetSessions(string? connectionBroker = null) =>
            ProcessRequest<List<RdsSession>>("RDS/GetSessions");

        public Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest("RDS/DisconnectSession", sessionInfo);

        public Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest("RDS/LogOffSession", sessionInfo);

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
                var content = await response.Content.ReadAsStringAsync();
                if (!content.TrimStart().StartsWith("{"))
                    throw new RdsServiceException($"An error occurred calling the RDS service, {response.StatusCode}, {response.Content.ReadAsStringAsync()}");
                var result = JsonSerializer.Deserialize<RdsServiceException>(content);
                if (result != null) throw result;
                throw new RdsServiceException($"An error occurred calling the RDS service, {response.StatusCode}, {response.Content.ReadAsStringAsync()}");
            }
        }
    }
}