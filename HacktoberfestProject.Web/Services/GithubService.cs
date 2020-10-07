using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
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
            _logger.LogTrace($"Sending request to Github for pull requests on repository: {name}");
            var prs = await _client.PullRequest.GetAllForRepository(owner, name, new PullRequestRequest() { State = ItemStateFilter.All });

            return prs.Select(pr => new Models.DTOs.PullRequest(pr.Number, pr.Url)).ToList();
        }

        public async Task<ServiceResponse<IEnumerable<string>>> SearchOwners(string owner, int limit)
        {
            var searchResults = new List<string>();

            if (!string.IsNullOrWhiteSpace(owner))
            {
                _logger.LogTrace($"Sending request to Github for owners like: {owner}");
                var users = await _client.Search.SearchUsers(new SearchUsersRequest(owner)
                {
                    AccountType = AccountSearchType.User,
                    In = new[] { UserInQualifier.Username }
                });

                if (users == null || !users.Items.Any())
                {
                    return new ServiceResponse<IEnumerable<string>>
                    {
                        ServiceResponseStatus = ServiceResponseStatus.NotFound,
                        Message = $"No results found for search term {owner}!"
                    };
                }

                users.Items.ToList().ForEach(u => searchResults.Add(u.Login));
            }

            return new ServiceResponse<IEnumerable<string>>
            {
                Content = searchResults.Take(limit),
                ServiceResponseStatus = ServiceResponseStatus.Ok
            };
        }
    }
}
