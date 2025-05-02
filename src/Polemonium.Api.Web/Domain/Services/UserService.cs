using Polemonium.Api.Web.Domain.Entities;
using Polemonium.Api.Web.Domain.Repositories;
using System;

namespace Polemonium.Api.Web.Domain.Services
{
    public interface IUserService
    {
        AppUser CreateUser();
    }

    public class UserService : IUserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public AppUser CreateUser()
        {
            var appUser = new AppUser();
            appUser.CreatedOn = DateTime.UtcNow;

            userRepository.CreateUser(appUser);


            return appUser;
        }
    }
}
