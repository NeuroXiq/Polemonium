using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Web.Common;
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
        private IHostService hostService;

        public HostController(IHostService hostService)
        {
            this.hostService = hostService;
        }

        [HttpPut, Route("set-vote"), Authorize]
        public async Task<object> SetVote(SetVoteModel model)
        {
            await hostService.SetVote(model.Host, (Domain.Enums.HostVoteType)model.Vote);

            return null;
        }

        [HttpGet, Route("votes")]
        public IList<HostVoteDto> GetVotesStatus([FromQuery] string[] hosts)
        {
            if (hosts == null || hosts.Length == 0) return new List<HostVoteDto>();
            string[] validSites = hosts
                .Where(h => Uri.CheckHostName(h) != UriHostNameType.Unknown)
                .Select(t => t.Trim())
                .Distinct()
                .ToArray();

            if (validSites.Length == 0) return Array.Empty<HostVoteDto>();

            var r = new Random();

            return hosts.Select(t => new HostVoteDto { DnsName = t, VoteDownCount = r.Next(100), VoteUpCount = r.Next(100) }).ToList();
        }
    }
}
