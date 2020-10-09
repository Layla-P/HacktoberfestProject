using System.Linq;

using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Services;

using Microsoft.Extensions.Logging;

using Xunit;

namespace HacktoberfestProject.Tests.Services
{
	public class GithubServiceTests
	{
		private readonly IGithubService _githubService = new GithubService(new Logger<GithubService>(new LoggerFactory()));

		[Fact]
		public async void GetRepos_GivenOwner_Should_ReturnRepositories()
		{
			var repos = await _githubService.GetRepos(Constants.OWNER);

			Assert.NotEmpty(repos);
		}

		[Fact]
		public async void GetPrs_GivenOwnerAndReponame_Should_ReturnPrs()
		{
			var prs = await _githubService.GetPullRequestsForRepo(Constants.OWNER, Constants.REPO_NAME);
			Assert.NotEmpty(prs);
		}

		[Fact]
		public async void SearchOwners_GivenOwnerAndLimit()
		{
			var serviceResponse = await _githubService.SearchOwners(Constants.OWNER, 5);

			Assert.NotNull(serviceResponse);
			Assert.Equal(ServiceResponseStatus.Ok, serviceResponse.ServiceResponseStatus);
			Assert.Equal(5, serviceResponse.Content.Count());
			Assert.Equal(Constants.OWNER, serviceResponse.Content.FirstOrDefault());
		}

		[Fact]
		public async void ValidatePrStatus_GivenOwnerAndRerpositoyNameAndIdValidPr_Should_ReturnServiceResponseWithPrStatus()
		{
			var serviceResponse = await _githubService.ValidatePrStatus(Constants.OWNER, Constants.REPO_NAME, 35);

			Assert.NotNull(serviceResponse);
			Assert.Equal(ServiceResponseStatus.Ok, serviceResponse.ServiceResponseStatus);
			Assert.Equal(PrStatus.Valid, serviceResponse.Content);
		}
	}
}
