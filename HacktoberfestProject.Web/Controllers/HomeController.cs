using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models;
using HacktoberfestProject.Web.Models.Entities;

namespace HacktoberfestProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITableContext _tableContext;
        private const string GitHubUsernameClaimType = "urn:github:login";
        private const string EmailClaimType = "urn:github:email";

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor, ITableContext tableContext)
        {
            _logger = logger;
            _contextAccessor = contextAccessor;
            _tableContext = tableContext;
        }

        public async Task<IActionResult> Index()
        {
            UserinfoViewModel model = null;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == GitHubUsernameClaimType).Value;
                var user = await _tableContext.RetrieveEnitityAsync<UserEntity>(UserEntity.PARTITIONKEY, username);
                if (user != null)
                {
                    model = new UserinfoViewModel()
                    {
                        Username = user.Username
                    };
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            // How to access claims - left here for reference         
            // ViewBag.Username = _contextAccessor.HttpContext.User
            //    .Claims.FirstOrDefault(c => c.Type == GitHubUsernameClaimType).Value;
  
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
