using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Octokit;

using HacktoberfestProject.Web.Tools;

namespace HacktoberfestProject.Web.Services
{
    public class GithubService : IGithubService
    {
        private ILogger<GithubService> _logger;
        private GitHubClient _client = new GitHubClient(new ProductHeaderValue("HacktoberfestProject"));

        public GithubService(ILogger<GithubService> logger)
        {
            NullChecker.IsNotNull(logger, nameof(logger));
            _logger = logger;
        }

        public async Task<List<Models.DTOs.Repository>> GetRepos(string owner)
        {
            _logger.LogTrace($"Sending request to Github for repositories belonging to user: {owner}");
            var repositories = await _client.Repository.GetAllForUser(owner);

            return repositories.Select(r => new Models.DTOs.Repository(owner, r.Name, r.Url)).ToList();
        }

        public async Task<List<Models.DTOs.PullRequest>> GetPullRequestsForRepo(string owner, string name)
        {
            _logger.LogTrace($"Sending request to Github for pull requests on repositoy: {name}");
            var prs = await _client.PullRequest.GetAllForRepository(owner, name, new PullRequestRequest() { State = ItemStateFilter.All });

            return prs.Select(pr => new Models.DTOs.PullRequest(pr.Number, pr.Url)).ToList();
        }
    }
}
