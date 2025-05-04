using System;

namespace Polemonium.Api.Web.Domain.Entities
{
    public class WebsiteHostComment : EntityBase
    {
        public string Content { get; set; }
        public int AppUserId { get; set; }
        public int WebsiteHostId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
