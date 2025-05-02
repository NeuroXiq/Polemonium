using System;

namespace Polemonium.Api.Web.Domain.Entities
{
    public class AppUser : EntityBase
    {
        public string AuthToken { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
