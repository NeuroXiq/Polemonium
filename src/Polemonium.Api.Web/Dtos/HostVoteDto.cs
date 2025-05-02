namespace Polemonium.Api.Web.Dtos
{
    public class HostVoteDto
    {
        public string DnsName { get; set; }
        public int VoteUpCount { get; set; }
        public int VoteDownCount { get; set; }
    }
}
