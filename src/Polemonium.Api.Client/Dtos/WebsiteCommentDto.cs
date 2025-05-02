using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polemonium.Api.Client.Dtos
{
    public class WebsiteCommentDto
    {
        public int Id { get; set; }
        public int WebsiteHostId { get; set; }
        public int AppUserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
