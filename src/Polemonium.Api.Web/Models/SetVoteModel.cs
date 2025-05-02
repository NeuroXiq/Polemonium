using Polemonium.Api.Web.Dtos.Enums;

namespace Polemonium.Api.Web.Models
{
    public class SetVoteModel
    {
        public string Host { get; set; }
        public HostVoteType Vote { get; set; }
    }
}
