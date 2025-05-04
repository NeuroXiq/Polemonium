using Polemonium.Api.Web.Common;
using Polemonium.Api.Web.Domain.Services;

namespace Polemonium.Api.Web.Application
{
    public class CurrentUser : ICurrentUser
    {
        public int UserId => UserIdOrNull.HasValue ? UserIdOrNull.Value : throw new PValidationException("not authorized");
        public int? UserIdOrNull { get; private set; }

        public CurrentUser()
        {
            UserIdOrNull = null;
        }

        public void Set(int id)
        {
            UserIdOrNull = id;
        }
    }
}
