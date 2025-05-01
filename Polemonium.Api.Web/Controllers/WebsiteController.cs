using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Web.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Polemonium.Api.Web.Controllers
{
    public class WebsiteController : PolemoniumController
    {
        public WebsiteController()
        {
        
        }

        [HttpGet, Route("hosts-votes")]
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

            return hosts.Select(t => new HostVoteDto { Host = t, VoteDownCount = r.Next(100), VoteUpCount = r.Next(100) }).ToList();
        }
    }
}
