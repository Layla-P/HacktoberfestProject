using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Services;
using HacktoberfestProject.Web.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HacktoberfestProject.Web.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IGithubService _githubService;

        public SearchController(IGithubService githubService)
        {
            NullChecker.IsNotNull(githubService, nameof(githubService));
            _githubService = githubService;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Search([FromQuery] string owner, [FromQuery] int limit = 100)
        {
            var owners = await _githubService.SearchOwners(owner);
            return owners.Take(limit);
        }
    }
}
