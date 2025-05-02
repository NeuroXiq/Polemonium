using Npgsql;
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
        protected IPolemoniumInfrastructure infrastructure;
        protected NpgsqlConnection Connection { get; private set; }

        public RepositoryBase(IPolemoniumInfrastructure infrastructure)
        {
            this.infrastructure = infrastructure;
            Connection = new NpgsqlConnection(infrastructure.ConnectionString);
        }
    }
}
