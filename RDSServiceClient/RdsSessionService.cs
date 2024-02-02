using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using RDSServiceLibrary;
using RDSServiceLibrary.Interfaces;
using RDSServiceLibrary.Models;

namespace RDSServiceClient
{
    public class RdsSessionService : IRdsSessionService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<RdsServiceClientOptions> _options;

        public RdsSessionService(IHttpClientFactory httpClientFactory, IOptions<RdsServiceClientOptions> options)
        {
            _options = options;
            _httpClient = httpClientFactory.CreateClient("RdsServiceClient");
            _httpClient.BaseAddress = new Uri(options.Value.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<string> GetActiveManagementServer(string? connectionBroker = null) =>
            ProcessRequest<string>(_options.Value.ActiveManagementServerUrl);

        public Task<List<RdsSession>> GetSessions(string? connectionBroker = null) =>
            ProcessRequest<List<RdsSession>>(_options.Value.SessionsUrl);

        public Task<bool> DisconnectSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest(_options.Value.DisconnectUrl, sessionInfo);

        public Task<bool> LogOffSession(SessionInfo sessionInfo, string? connectionBroker = null) =>
            ProcessPostRequest(_options.Value.LogoffUrl, sessionInfo);

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