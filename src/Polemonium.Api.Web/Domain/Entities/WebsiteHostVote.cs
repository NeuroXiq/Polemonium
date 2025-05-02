namespace Polemonium.Api.Web.Domain.Entities
{
    public class WebsiteHostVote : EntityBase
    {
        public int AppUserId { get; set; }
        public byte Vote { get; set; }
    }
}
