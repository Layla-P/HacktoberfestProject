using Microsoft.Extensions.Logging;
using Xunit;

using HacktoberfestProject.Web.Services;

namespace HacktoberfestProject.Tests.Services
{
    public class GithubServiceTests
	{
		[Fact]
		public async void RunTableStorageTests()
		{
			var githubService = new GithubService(new Logger<GithubService>(new LoggerFactory()));
			var repos = await githubService.GetRepos(Constants.OWNER);
			var prs = await  githubService.GetPullRequestsForRepo(Constants.OWNER, Constants.REPO_NAME);

			Assert.NotEmpty(repos);
			Assert.NotEmpty(prs);
		}
	}
}
