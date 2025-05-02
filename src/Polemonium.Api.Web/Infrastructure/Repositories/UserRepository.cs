using Dapper;
using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.Repositories;
using Polemonium.Api.Web.Infrastructure.Shared;
using System;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IPolemoniumInfrastructure infrastructure) : base(infrastructure)
        {
        }

        public async Task CreateUser(AppUser user)
        {
            user.Id = await Connection.ExecuteScalarAsync<int>(
                "INSERT INTO app_user(created_on) VALUES (@CreatedOn) RETURNING id",
                user);
        }
    }
}
