using Polemonium.Api.Web.Domain.Enums;

namespace Polemonium.Api.Web.Domain.ValueObjects
{
    public class VwWebsiteHostDetails
    {
        public int Id { get; set; }
        public string DnsName { get; set; }
        public int VoteUpCount { get; set; }
        public int VoteDownCount { get; set; }
        public int CommentsCount { get; set; }
        public HostVoteType? UserVote { get; set; }

        public VwWebsiteHostDetails() { }
    }
}
