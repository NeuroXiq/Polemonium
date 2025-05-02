using Npgsql;
using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Domain.ValueObjects;
using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using Dapper;
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

        public async Task CreateWebsiteHostVote(WebsiteHostVote hostVote)
        {
            hostVote.Id = await Connection.ExecuteScalarAsync<int>(
                "INSERT INTO website_host_vote(app_user_id, website_host_id, vote) " +
                "values(@AppUserId, @WebsiteHostId, @Vote) RETURNING id",
                hostVote);
        }

        public Task<WebsiteHost> GetByDnsName(string dnsName)
        {
            return Connection.QueryFirstOrDefaultAsync<WebsiteHost>(
                $"{SQL_SelectWebsiteHost} WHERE dns_name = @dnsName",
                new { dnsName });
        }

        public async Task<VwWebsiteHostVotes> GetVotesCounts(string hosts)
        {
            throw new Exception();
            // throw new NotImplementedException();
        }

        public async Task CreateWebsiteHost(WebsiteHost host)
        {
            host.Id = await Connection.ExecuteScalarAsync<int>(
                @"INSERT INTO website_host(dns_name) VALUES (@DnsName)
                RETURNING id",
                host);
        }

        const string SQL_SelectWebsiteHost = "SELECT id as Id, dns_name as DnsName FROM website_host";
    }
}
