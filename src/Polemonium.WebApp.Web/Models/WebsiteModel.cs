using Polemonium.Api.Client.Dtos;

namespace Polemonium.WebApp.Web.Models
{
    public class WebsiteModel
    {
        public WebsiteDetailsDto WebsiteDetails { get; set; }
        public IList<WebsiteCommentDto> Comments { get; set; }
        
    }
}
