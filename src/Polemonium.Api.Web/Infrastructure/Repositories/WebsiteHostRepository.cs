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

        public Task<WebsiteHostVote> GetWebsiteHostVote(string dnsName, int appUserId)
        {
            return Connection.QueryFirstOrDefaultAsync<WebsiteHostVote>($@"
{SQL_SelectWebsiteHostVote("v")}
JOIN website_host wh on wh.id = v.website_host_id
WHERE v.app_user_id = @appUserId AND wh.dns_name = @dnsName"
                , new { dnsName, appUserId });
        }

        public Task UpdateWebsiteHostVote(WebsiteHostVote existingVote)
        {
            return Connection.ExecuteAsync(@"
UPDATE website_host_vote
SET 
app_user_id = @AppUserId,
website_host_id = @WebsiteHostId,
vote = @Vote
WHERE id = @Id
",
                existingVote);
        }

        static string SQL_SelectWebsiteHostVote(string alias)
        {
            string p = alias == null ? "" : $"{alias}.";

            return $@"
SELECT
{p}id as Id,
{p}app_user_id as AppUserId,
{p}website_host_id as WebsiteHostId,
{p}vote as Vote
FROM website_host_vote {alias}
";
        }

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

        public Task<VwWebsiteHostDetails> VwGetWebsiteHostDetailsByDnsName(string dnsName, int? appUserId)
        {
            return Connection.QueryFirstOrDefaultAsync<VwWebsiteHostDetails>(@"
SELECT
wh.id as Id,
wh.dns_name as DnsName,
(SELECT COUNT(*) FROM website_host_vote whv WHERE whv.website_host_id = wh.id AND vote = 1::char(1)) as VoteUpCount,
(SELECT COUNT(*) FROM website_host_vote whv WHERE whv.website_host_id = wh.id AND vote = 2::char(1)) as VoteDownCount,
(SELECT COUNT(*) FROM website_host_comment whc where whc.website_host_id = wh.id) as CommentsCount,
(
SELECT vote FROM website_host_vote whv
WHERE whv.website_host_id = wh.id
AND app_user_id = @appUserId
LIMIT 1
)
as UserVote
FROM website_host wh
WHERE wh.dns_name = @dnsName

", new { dnsName, appUserId });
        }




        const string SQL_SelectWebsiteHost = "SELECT id as Id, dns_name as DnsName FROM website_host";
    }
}
