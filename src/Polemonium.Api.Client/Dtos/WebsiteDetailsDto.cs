using Polemonium.Api.Client.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Client.Dtos
{
    public class WebsiteHostDetailsDto
    {
        public int Id { get; set; }
        public string DnsName { get; set; }
        public int VoteUpCount { get; set; }
        public int VoteDownCount { get; set; }
        public int CommentsCount { get; set; }
        public HostVoteType? UserVote { get; set; }
    }
}
