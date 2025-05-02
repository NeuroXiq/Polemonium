namespace Polemonium.Api.Web.Domain.Services
{
    public interface ICurrentUser
    {
        int UserId { get; }

        void Set(int id);
    }
}
