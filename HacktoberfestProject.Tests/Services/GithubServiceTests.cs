using System.Linq;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;
using Xunit;

using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Services;
using HacktoberfestProject.Web.Services.Configuration;

namespace HacktoberfestProject.Tests.Services
{
	public class GithubServiceTests
	{
		private readonly IGithubService _githubService;

		public GithubServiceTests()
		{
			var config = new ConfigurationBuilder()
				.AddUserSecrets(Assembly.Load(new AssemblyName("HacktoberfestProject.Web")))
				.AddEnvironmentVariables()
				.Build();

			var client = new GitHubClient(new ProductHeaderValue("HacktoberfestProject"));
			var configuration = Options.Create( new GithubConfiguration { ClientId = config["GitHub:clientId"], ClientSecret = config["GitHub:clientSecret"]});
			_githubService = new GithubService(new Logger<GithubService>(new LoggerFactory()), configuration, client);
		}

		/// <summary>
		/// This checks that we are using authenticated calls to Github as it will fail API Limits if we are not.
		/// </summary>
		[Fact]
		public async void GetRepos_GivenOwner_Loop60_ReturnRepositories()
		{
			for (int i = 0; i < 61; i++)
			{
				var repos = await _githubService.GetRepos(Constants.OWNER);
				Assert.NotEmpty(repos);
			}
		}

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
			Assert.Equal(Constants.OWNER, serviceResponse.Content.FirstOrDefault().Name);
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
