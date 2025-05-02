using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.ValueObjects;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Domain.Repositories
{
    public interface IWebsiteHostRepository
    {
        Task<VwWebsiteHostVotes> GetVotesCounts(string hosts);
    }
}
