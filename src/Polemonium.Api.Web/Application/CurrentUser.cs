using Polemonium.Api.Web.Domain.Services;

namespace Polemonium.Api.Web.Application
{
    public class CurrentUser : ICurrentUser
    {
        public int UserId { get; private set; }

        public void Set(int id)
        {
            UserId = id;
        }
    }
}
