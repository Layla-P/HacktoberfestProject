using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Services;
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
            _githubService = githubService;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Search([FromQuery] string owner, [FromQuery] int limit = 100)
        {
            return (await _githubService.SearchOwners(owner))
                .Take(limit);
        }
    }
}
