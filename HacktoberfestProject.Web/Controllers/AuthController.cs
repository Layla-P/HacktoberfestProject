using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HacktoberfestProject.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }
    }
}