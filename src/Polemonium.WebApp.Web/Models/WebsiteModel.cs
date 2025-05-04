using Polemonium.Api.Client.Dtos;
using System.Collections.Generic;

namespace Polemonium.WebApp.Web.Models
{
    public class WebsiteModel
    {
        public WebsiteHostDetailsDto WebsiteDetails { get; set; }
        public IList<WebsiteCommentDto> Comments { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
