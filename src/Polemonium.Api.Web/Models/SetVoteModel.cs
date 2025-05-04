using Polemonium.Api.Web.Dtos.Enums;

namespace Polemonium.Api.Web.Models
{
    public class SetVoteModel
    {
        public string DnsName { get; set; }
        public HostVoteType Vote { get; set; }
    }
}
