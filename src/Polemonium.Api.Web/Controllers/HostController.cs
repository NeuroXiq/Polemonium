using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Client.Dtos;
using Polemonium.Api.Client.Dtos.Enums;
using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Domain.Services;
using Polemonium.Api.Web.Dtos;
using Polemonium.Api.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Controllers
{
    public class HostController : PolemoniumController
    {
        private ICurrentUser user;
        private IHostService hostService;
        private IWebsiteHostRepository websiteHostRepository;

        public HostController(
            IHostService hostService,
            IWebsiteHostRepository websiteHostRepository,
            ICurrentUser user)
        {
            this.user = user;
            this.hostService = hostService;
            this.websiteHostRepository = websiteHostRepository;
        }

        [HttpGet, Route("website-host-details/{dnsName}")]
        public async Task<WebsiteHostDetailsDto> GetWebsiteHostDetails(string dnsName)
        {
            var details = await websiteHostRepository.VwGetWebsiteHostDetailsByDnsName(dnsName, user.UserIdOrNull);

            if (details == null)
            {
                details = new Domain.ValueObjects.VwWebsiteHostDetails()
                {
                    DnsName = dnsName
                };
            }

            return new WebsiteHostDetailsDto
            {
                CommentsCount = details.CommentsCount,
                DnsName = details.DnsName,
                Id = details.Id,
                VoteDownCount = details.VoteDownCount,
                VoteUpCount = details.VoteUpCount,
                UserVote = (HostVoteType?)details.UserVote
            };
        }

        [HttpGet, Route("comments")]
        public async Task<IList<Client.Dtos.WebsiteCommentDto>> GetComments(string dnsName, int skip, int take)
        {
            if (skip < 0) throw new PValidationException("skip invalid value");
            if (take < 0 || take > 100) throw new PValidationException("take invalid value");

            var comments = await websiteHostRepository.GetWebsiteHostCommentsByDnsName(dnsName, skip, take);

            return comments.Select(c => new Client.Dtos.WebsiteCommentDto
            {
                AppUserId = c.AppUserId,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                Id = c.Id,
                WebsiteHostId = c.WebsiteHostId
            }).ToList();
        }

        [HttpPost, Route("add-comment"), Authorize]
        public async Task<int> AddComment(AddCommentModel model)
        {
            return await hostService.AddCommentAsync(model.DnsName, model.Content);
        }

        [HttpPut, Route("set-vote"), Authorize]
        public async Task<object> SetVote(SetVoteModel model)
        {
            await hostService.SetVote(model.DnsName, (Domain.Enums.HostVoteType)model.Vote);

            return null;
        }

        [HttpGet, Route("votes")]
        public IList<HostVoteDto> GetVotesStatus([FromQuery] string[] dnsName)
        {
            if (dnsName == null || dnsName.Length == 0) return new List<HostVoteDto>();
            string[] validSites = dnsName
                .Where(h => Uri.CheckHostName(h) != UriHostNameType.Unknown)
                .Select(t => t.Trim())
                .Distinct()
                .ToArray();

            if (validSites.Length == 0) return Array.Empty<HostVoteDto>();

            var r = new Random();

            return dnsName.Select(t => new HostVoteDto { DnsName = t, VoteDownCount = r.Next(100), VoteUpCount = r.Next(100) }).ToList();
        }
    }
}
