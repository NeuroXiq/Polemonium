using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Enums;
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
    }

    public class HostService : IHostService
    {
        private IHostRepository hostRepository;

        public HostService(IHostRepository hostRepository)
        {
            this.hostRepository = hostRepository;
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
            if (string.IsNullOrWhiteSpace(host) || Uri.CheckHostName(host) == UriHostNameType.Unknown)
            {
                throw new PValidationException("invalid host name");
            }

            if (!Enum.IsDefined(vote)) throw new PValidationException("invalid vote value");
        }

        public Task UpdateComment()
        {
            throw new System.NotImplementedException();
        }
    }
}
