using Polemonium.Api.Client.Dtos;
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
        Task<string> RegisterAsync();
        Task<WebsiteDetailsDto> GetWebsiteDetailsAsync(string dnsName);
        Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(string dnsName, int skip, int take);
    }

    public class PolemoniumApiClient : IPolemoniumApiClient
    {
        HttpClient httpClient;

        public PolemoniumApiClient(string apiUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(apiUrl);
        }

        public async Task<int> AddWebsiteCommentAsync(string authToken, string dnsName, string content)
        {
            var httpResult = await httpClient.PostAsJsonAsync("/api/host/add-comment", new { dnsName, content });

            return await httpResult.Content.ReadFromJsonAsync<int>();
        }

        public async Task<string> RegisterAsync()
        {
            var httpResult = await httpClient.PostAsync("/api/auth/register", null);

            var token = await httpResult.Content.ReadFromJsonAsync<string>();

            return token;
        }

        public async Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(string dnsName, int skip, int take)
        {
            var httpResult = await httpClient.GetAsync($"/api/host/comments?dnsName={dnsName}&skip={skip}&take={take}");

            return await httpResult.Content.ReadFromJsonAsync<IList<WebsiteCommentDto>>();
        }

        public async Task<WebsiteDetailsDto> GetWebsiteDetailsAsync(string dnsName)
        {
            return new WebsiteDetailsDto
            {
                Id = 1,
                CommentsCount = 15,
                DnsName = dnsName,
                VoteDownCount = 5,
                VoteUpCount = 10
            };
        }
    }
}
