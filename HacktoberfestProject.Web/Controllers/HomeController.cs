using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using HacktoberfestProject.Web.Models;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Services;

namespace HacktoberfestProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITableService _tableService;
        private const string GitHubUsernameClaimType = "urn:github:login";
        private const string EmailClaimType = "urn:github:email";

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor, ITableService tableService)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _tableService = tableService;
        }

        public async Task<IActionResult> Index()
        {
            User user = null;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == GitHubUsernameClaimType).Value;
                var response = await _tableService.GetUserByUsernameAsync(username);
                
                if (response.ServiceResponseStatus == Models.Enums.ServiceResponseStatus.Ok)
                {
                    user = response.Content;
                }
            }
            return View(user);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            //How to access claims -left here for reference
            ViewBag.Username = _contextAccessor.HttpContext.User
                .Claims.FirstOrDefault(c => c.Type == GitHubUsernameClaimType).Value;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
