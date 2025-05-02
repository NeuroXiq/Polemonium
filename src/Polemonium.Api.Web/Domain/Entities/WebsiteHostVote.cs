namespace Polemonium.Api.Web.Domain.Entities
{
    public class WebsiteHostVote : EntityBase
    {
        public int AppUserId { get; set; }
        public int WebsiteHostId { get; set; }
        public byte Vote { get; set; }

        public WebsiteHostVote(int appUserId, int websiteHostId, byte vote)
        {
            AppUserId = appUserId;
            WebsiteHostId = websiteHostId;
            Vote = vote;
        }
    }
}
