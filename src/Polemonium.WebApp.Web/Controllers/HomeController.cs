using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Client.Client;
using Polemonium.Api.Client.Dtos;
using Polemonium.WebApp.Web.Models;
using System.Diagnostics;

namespace Polemonium.WebApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IPolemoniumApiClient polemoniumApiClient;

        public HomeController(ILogger<HomeController> logger, IPolemoniumApiClient polemoniumApiClient)
        {
            this.logger = logger;
            this.polemoniumApiClient = polemoniumApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet, Route("website/{dnsName}")]
        public async Task<IActionResult> Website(string dnsName)
        {
            var details = await polemoniumApiClient.GetWebsiteDetailsAsync(dnsName);
            var comments = await polemoniumApiClient.GetWebsiteCommentsAsync(details.Id, 0, 10);

            var model = new WebsiteModel()
            {
                Comments = comments,
                WebsiteDetails = details,
                CurrentPage = 2,
                PageCount = 55
            };

            return View(model);
        }

        [HttpGet, Route("website/add-comment/{dnsName}")]
        public async Task<IActionResult> WebsiteAddComment()
        {
            return null;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
