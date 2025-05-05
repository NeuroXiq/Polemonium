using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.Enums;
using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Domain.Services
{
    public interface IHostService
    {
        Task SetVote(string host, HostVoteType vote);
        Task<int> AddComment(string content);
        Task UpdateComment();
        Task DeleteComment();
        Task<int> AddCommentAsync(string dnsName, string content);
    }

    public class HostService : IHostService
    {
        private IWebsiteHostRepository hostRepository;
        private ICurrentUser user;

        public HostService(IWebsiteHostRepository hostRepository, ICurrentUser user)
        {
            this.hostRepository = hostRepository;
            this.user = user;
        }

        public Task<int> AddComment(string content)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteComment()
        {
            throw new System.NotImplementedException();
        }

        public async Task SetVote(string host, HostVoteType vote)
        {
            if (!Enum.IsDefined(vote)) throw new PValidationException("invalid vote value");

            WebsiteHost websiteHost = await GetOrCreateWebsiteHost(host);

            WebsiteHostVote existingVote = await hostRepository.GetWebsiteHostVote(host, user.UserId);

            if (existingVote != null && existingVote.Vote == (byte)vote)
            {
                await hostRepository.DeleteWebsiteHostVote(existingVote.Id);
            }
            else if (existingVote != null && existingVote.Vote == (byte)vote)
            {
                existingVote.Vote = (byte)vote;
                await hostRepository.UpdateWebsiteHostVote(existingVote);

                return;
            }
            else
            {
                WebsiteHostVote hostVote = new WebsiteHostVote(user.UserId, websiteHost.Id, (byte)vote);

                await hostRepository.CreateWebsiteHostVote(hostVote);
            }
        }

        async Task<WebsiteHost> GetOrCreateWebsiteHost(string dnsName)
        {
            if (string.IsNullOrWhiteSpace(dnsName)) throw new PValidationException("empty dnsName");

            dnsName = dnsName?.Trim();
            if (string.IsNullOrWhiteSpace(dnsName) || Uri.CheckHostName(dnsName) == UriHostNameType.Unknown)
            {
                throw new PValidationException("invalid host name");
            }

            WebsiteHost host = await hostRepository.GetByDnsName(dnsName);

            if (host == null)
            {
                host = new WebsiteHost(dnsName);
                await hostRepository.CreateWebsiteHost(host);
            }

            return host;
        }

        public Task UpdateComment()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> AddCommentAsync(string dnsName, string content)
        {
            if (string.IsNullOrWhiteSpace(content)) throw new PValidationException("content is empty");
            if (content.Length > 512) throw new PValidationException("content length exceed 512 chars");

            WebsiteHost websiteHost = await GetOrCreateWebsiteHost(dnsName);

            var comment = new WebsiteHostComment()
            {
                AppUserId = user.UserId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
                WebsiteHostId = websiteHost.Id
            };

            await hostRepository.CreateCommentAsync(comment);

            return comment.Id;
        }
    }
}
