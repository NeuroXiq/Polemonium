using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Domain.ValueObjects;
using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Infrastructure.Repositories
{
    public class WebsiteHostRepository : RepositoryBase, IWebsiteHostRepository
    {
        public WebsiteHostRepository(IPolemoniumInfrastructure infrastructure) : base(infrastructure)
        {
        }

        public async Task<VwWebsiteHostVotes> GetVotesCounts(string hosts)
        {
            throw new Exception();
            // throw new NotImplementedException();
        }
    }
}
