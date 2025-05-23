﻿using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Domain.Repositories
{
    public interface IWebsiteHostRepository
    {
        Task CreateCommentAsync(WebsiteHostComment comment);
        Task CreateWebsiteHost(WebsiteHost host);
        Task CreateWebsiteHostVote(WebsiteHostVote hostVote);
        Task<WebsiteHost> GetByDnsName(string dnsName);
        Task<WebsiteHost> GetByIdAsync(int websiteHostId);
        Task<IList<WebsiteHostComment>> GetWebsiteHostCommentsByDnsName(string dnsName, int skip, int take);
        Task<IEnumerable<VwWebsiteHostVotes>> GetVotesCounts(string[] dnsNames);
        Task<VwWebsiteHostDetails> VwGetWebsiteHostDetailsByDnsName(string dnsName, int? appUserId);
        Task<WebsiteHostVote> GetWebsiteHostVote(string host, int userId);
        Task UpdateWebsiteHostVote(WebsiteHostVote existingVote);
        Task DeleteWebsiteHostVote(int id);
    }
}
