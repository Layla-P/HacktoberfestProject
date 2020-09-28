using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HacktoberfestProject.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HacktoberfestProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private const string GitHubUsernameClaimType = "urn:github:login";
        private const string EmailClaimType = "urn:github:email";

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor)
        {
            
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
               //How to access claims - left here for reference         
            //ViewBag.Username = _contextAccessor.HttpContext.User
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
