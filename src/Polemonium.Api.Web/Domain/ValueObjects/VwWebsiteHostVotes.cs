namespace Polemonium.Api.Web.Domain.ValueObjects
{
    public class VwWebsiteHostVotes
    {
        public string HostDnsName { get; set; }
        public int VotesUp { get; set; }
        public int VotesDown { get; set; }
    }
}
