using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Infrastructure.Repositories
{
    public class RepositoryBase
    {
        private IPolemoniumInfrastructure infrastructure;

        public RepositoryBase(IPolemoniumInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
        }
    }
}
