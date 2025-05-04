using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Polemonium.Api.Client.Client;
using Polemonium.Api.Client.Common;
using Polemonium.Api.Client.Dtos;
using Polemonium.Shared.Auth;
using Polemonium.WebApp.Web.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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

        [Route("/error")]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            exceptionFeature.Endpoint.ToString();

            string errorMessage = "";

            if (exceptionFeature.Error is PolemoniumApiResponseException)
            {
                errorMessage = (exceptionFeature.Error as PolemoniumApiResponseException).Error;
            }

            ErrorViewModel model = new ErrorViewModel()
            {
                ErrorMessage = errorMessage,
                RequestId = HttpContext.TraceIdentifier,
                Path = exceptionFeature.Path.ToString(),
                ExceptionType = exceptionFeature.Error?.GetType().Name,
                Endpoint = exceptionFeature.Endpoint.ToString(),
                RouteValues = string.Join(", ", exceptionFeature.RouteValues?.ToArray().Select(t => $"{t.Key}: {t.Value}").ToArray() ?? new string[0])
            };

            return View("Error", model);
        }

        [HttpGet, Route("/search-website")]
        public async Task<IActionResult> SearchWebsite(string dnsName)
        {
            if (string.IsNullOrWhiteSpace(dnsName)) return Redirect("/");

            return Redirect($"/website/{dnsName.Trim().ToLower()}");
        }

        [HttpGet, Route("website/{dnsName}")]
        public async Task<IActionResult> Website(string dnsName, int comment_page)
        {
            comment_page = comment_page < 1 ? 1 : comment_page;
            int commentPerPage = 5;
            var details = await polemoniumApiClient.GetWebsiteDetailsAsync(GetAuthTokenIfExists(), dnsName);
            var comments = await polemoniumApiClient.GetWebsiteCommentsAsync(dnsName, commentPerPage * (comment_page - 1), commentPerPage);

            var model = new WebsiteModel()
            {
                Comments = comments,
                WebsiteDetails = details,
                CurrentPage = comment_page,
                PageCount = (int)Math.Ceiling((double)details.CommentsCount / commentPerPage)
            };

            return View(model);
        }

        [HttpPost, Route("/website/set-vote")]
        public async Task<IActionResult> SetVote(string dnsName, int vote)
        {
            await polemoniumApiClient.SetVote(await GetAuthTokenOrRegister(), dnsName, vote);

            return Redirect($"/website/{dnsName}");
        }

        [HttpPost, Route("/website/add-comment")]
        public async Task<IActionResult> WebsiteAddComment(string dnsName, string content)
        {
            await polemoniumApiClient.AddWebsiteCommentAsync(await GetAuthTokenOrRegister(), dnsName, content);

            return Redirect($"/website/{dnsName}");
        }

        private string GetAuthTokenIfExists()
        {
            if (HttpContext.Request.Cookies.TryGetValue(AuthShared.AuthCookieName, out var token))
            {
                return token;
            }

            return null;
        }

        private async Task<string> GetAuthTokenOrRegister()
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
