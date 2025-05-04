using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Client.Dtos;
using Polemonium.Api.Web.Application;
using System.Threading.Tasks;

namespace Polemonium.Api.Web.Controllers
{
    public class AuthController : PolemoniumController
    {
        private IPAuthhentication pauthentication;

        public AuthController(IPAuthhentication pauthentication)
        {
            this.pauthentication = pauthentication;
        }

        [HttpPost, Route("register")]
        public RegisterResultDto Register()
        {
            return new RegisterResultDto { Token = pauthentication.RegisterNewUser() };
        }
    }
}
