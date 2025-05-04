using Polemonium.Api.Web.Common;

namespace Polemonium.Api.Web.Domain.Services
{
    public interface ICurrentUser
    {
        int? UserIdOrNull { get; }
        int UserId { get; }

        void Set(int id);
    }
}
