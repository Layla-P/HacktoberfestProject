using System.Collections.Generic;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Services;
using HacktoberfestProject.Web.Tools;

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
		public async Task<ServiceResponse<IEnumerable<TypeaheadResult>>> Search([FromQuery] string owner, 
			[FromQuery] int limit = 100)
		{
			var searchResponse = await _githubService.SearchOwners(owner, limit);
			return searchResponse;
		}
	}
}
