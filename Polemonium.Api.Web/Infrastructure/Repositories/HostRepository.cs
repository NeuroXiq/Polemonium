using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Infrastructure.Repositories
{
    public interface IHostRepository
    {
        
    }

    public class HostRepository : RepositoryBase, IHostRepository
    {
        public HostRepository(IPolemoniumInfrastructure infrastructure) : base(infrastructure)
        {
        }
    }
}
