using Polemonium.Api.Web.Domain.Entities;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Domain.Repositories
{
    public interface IUserRepository
    {
        Task CreateUser(AppUser user);
    }
}
