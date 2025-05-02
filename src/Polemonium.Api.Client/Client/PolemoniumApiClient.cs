using Polemonium.Api.Client.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Client.Client
{
    public interface IPolemoniumApiClient
    {
        Task<WebsiteDetailsDto> GetWebsiteDetailsAsync(string dnsName);
        Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(int websiteId, int skip, int take);
    }

    public class PolemoniumApiClient : IPolemoniumApiClient
    {
        HttpClient httpClient;

        public PolemoniumApiClient(string apiUrl)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(apiUrl);
        }

        public async Task<IList<WebsiteCommentDto>> GetWebsiteCommentsAsync(int websiteId, int skip, int take)
        {
            return null;
        }

        public async Task<WebsiteDetailsDto> GetWebsiteDetailsAsync(string dnsName)
        {
            return new WebsiteDetailsDto
            {
                Id = 1,
                CommentsCount = 5,
                DnsName = dnsName,
                VoteDownCount = 5,
                VoteUpCount = 10
            };
        }
    }
}
