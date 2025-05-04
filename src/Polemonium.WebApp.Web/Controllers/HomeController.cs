using Microsoft.AspNetCore.Mvc;
using Polemonium.Api.Client.Client;
using Polemonium.Api.Client.Dtos;
using Polemonium.Shared.Auth;
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
            var comments = await polemoniumApiClient.GetWebsiteCommentsAsync(dnsName, 0, 10);

            var model = new WebsiteModel()
            {
                Comments = comments,
                WebsiteDetails = details,
                CurrentPage = 2,
                PageCount = 2
            };

            return View(model);
        }

        [HttpPost, Route("/website/set-vote")]
        public async Task<IActionResult> SetVote(string dnsName, int vote)
        {
            await polemoniumApiClient.SetVote(await GetAuthToken(), dnsName, vote);

            return Redirect($"/website/{dnsName}");
        }

        [HttpPost, Route("/website/add-comment")]
        public async Task<IActionResult> WebsiteAddComment(string dnsName, string content)
        {
            await polemoniumApiClient.AddWebsiteCommentAsync(await GetAuthToken(), dnsName, content);

            return Redirect($"/website/{dnsName}");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<string> GetAuthToken()
        {
            if (!HttpContext.Request.Cookies.TryGetValue(AuthShared.AuthCookieName, out var token))
            {
                token =( await polemoniumApiClient.RegisterAsync()).Token;
            }

            HttpContext.Response.Cookies.Append(AuthShared.AuthCookieName, token);

            return token;
        }
    }
}
