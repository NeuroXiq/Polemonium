using Polemonium.Api.Client.Common;
using Polemonium.Api.Client.Dtos;
using Polemonium.Api.Client.Dtos.Common;
using Polemonium.Shared.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Client.Client
{
    public interface IPolemoniumApiClient
    {
        Task<int> AddWebsiteCommentAsync(string authToken, string dnsName, string content);
        Task<RegisterResultDto> RegisterAsync();
        Task<WebsiteHostDetailsDto> GetWebsiteDetailsAsync(string authToken, string dnsName);
        Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(string dnsName, int skip, int take);
        Task SetVote(string authToken, string dnsName, int vote);
    }

    public class PolemoniumApiClient : IPolemoniumApiClient
    {
        string baseAddress;
        private HttpClient GetClient(string authToken = null)
        {
            var c = new HttpClient();
            c.BaseAddress = new Uri(baseAddress);

            if (authToken != null) c.DefaultRequestHeaders.Add("Authorization", "Bearer " + authToken);

            return c;
        }

        public PolemoniumApiClient(string apiUrl)
        {
            baseAddress = apiUrl;
        }

        public async Task<int> AddWebsiteCommentAsync(string authToken, string dnsName, string content)
        {
            var httpResult = await GetClient(authToken).PostAsJsonAsync("/api/host/add-comment", new { dnsName, content });

            HandleError(httpResult);

            return await httpResult.Content.ReadFromJsonAsync<int>();
        }

        public async Task<RegisterResultDto> RegisterAsync()
        {
            var httpResult = await GetClient().PostAsync("/api/auth/register", null);

            HandleError(httpResult);

            return await httpResult.Content.ReadFromJsonAsync<RegisterResultDto>();
        }

        public async Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(string dnsName, int skip, int take)
        {
            var httpResult = await GetClient().GetAsync($"/api/host/comments?dnsName={dnsName}&skip={skip}&take={take}");

            HandleError(httpResult);

            return await httpResult.Content.ReadFromJsonAsync<IList<WebsiteCommentDto>>();
        }

        public async Task<WebsiteHostDetailsDto> GetWebsiteDetailsAsync(string authToken, string dnsName)
        {
            var httpResult = await GetClient(authToken).GetAsync($"/api/host/website-host-details/{dnsName}");

            HandleError(httpResult);

            return await httpResult.Content.ReadFromJsonAsync<WebsiteHostDetailsDto>();
        }

        public async Task SetVote(string authToken, string dnsName, int vote)
        {
            var req = new { dnsName, vote } as object;
            var result = await GetClient(authToken).PutAsJsonAsync("/api/host/set-vote", req);

            HandleError(result);
        }

        static void HandleError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = response.Content.ReadFromJsonAsync<PolemoniumApiErrorResponse>().Result;

                throw new PolemoniumApiResponseException() { Error = error.Error };
            }
        }
    }
}
