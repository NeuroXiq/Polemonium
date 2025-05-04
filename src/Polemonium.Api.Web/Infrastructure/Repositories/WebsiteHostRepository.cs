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

        public async Task<IList<WebsiteHostComment>> GetWebsiteHostCommentsByDnsName(string dnsName, int skip, int take)
        {
            var result = await Connection.QueryAsync<WebsiteHostComment>(@$"
{SQL_SelectComment("c")}  
JOIN website_host wh on wh.id = c.website_host_id
WHERE wh.dns_name = @dnsName
ORDER BY c.created_on DESC
LIMIT @limit
OFFSET @offset
",
                new { dnsName, limit = take, offset = skip });

            return result.ToList();
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

        public async Task CreateCommentAsync(WebsiteHostComment comment)
        {
            comment.Id = await Connection.ExecuteScalarAsync<int>(@"
INSERT INTO website_host_comment(content, app_user_id, website_host_id, created_on)
VALUES
(
@Content,
@AppUserId,
@WebsiteHostId,
@CreatedOn
)
RETURNING id
",
                comment);
        }

        public async Task<WebsiteHost> GetByIdAsync(int id)
        {
            return await Connection.QueryFirstOrDefaultAsync<WebsiteHost>($"{SQL_SelectWebsiteHost} WHERE id = @id", new { id });
        }

        const string SQL_SelectWebsiteHost = "SELECT id as Id, dns_name as DnsName FROM website_host";
            

        static string SQL_SelectComment(string alias)
        {
            string p = alias == null ? "" : $"{alias}.";
            return $@"
SELECT {p}id as Id,
{p}content as Content,
{p}app_user_id as AppUserId,
{p}website_host_id as WebsiteHostId,
{p}created_on as CreatedOn
FROM website_host_comment {alias}";
        }
    }
}
