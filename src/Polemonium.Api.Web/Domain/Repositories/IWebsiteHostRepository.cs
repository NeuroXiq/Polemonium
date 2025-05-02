using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.ValueObjects;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Domain.Repositories
{
    public interface IWebsiteHostRepository
    {
        Task CreateWebsiteHost(WebsiteHost host);
        Task CreateWebsiteHostVote(WebsiteHostVote hostVote);
        Task<WebsiteHost> GetByDnsName(string dnsName);
        Task<VwWebsiteHostVotes> GetVotesCounts(string hosts);
    }
}
