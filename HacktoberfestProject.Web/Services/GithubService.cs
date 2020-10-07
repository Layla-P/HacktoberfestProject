using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using Microsoft.Extensions.Logging;
using Octokit;

using HacktoberfestProject.Web.Tools;
using System;

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

        public async Task<ServiceResponse<PrStatus?>> ValidatePrStatus(string owner, string repo, int id)
        {
            /*
             * PRs count if:
             *       Submitted in a repo with the hacktoberfest topic AND
             *       during the month of October AND (
             *         The PR is merged OR
             *         The PR is labelled as hacktoberfest-accepted by a maintainer OR
             *         The PR has been approved
             *       )
             */

            var serviceResponse = new ServiceResponse<PrStatus?> { Content = PrStatus.Valid, ServiceResponseStatus = ServiceResponseStatus.Ok };

            try
            {

                var repoTags = await _client.Repository.GetAllTags(owner, repo);
                if (!repoTags.Any(rt => rt.Name == "hacktoberfest"))
                {
                    serviceResponse.Content = PrStatus.TopicInvalid;
                }

                var pr = await _client.PullRequest.Get(owner, repo, id);
                if (!(pr.CreatedAt.Year == 2020 && pr.CreatedAt.Date.Month == 10))
                {
                    serviceResponse.Content = PrStatus.InvalidDate;
                }


                if (!(pr.Merged || pr.Labels.Any(l => l.Name.ToLower() == "hacktoberfest-accepted") || await PrIsApproved(owner, repo, id)))
                {
                    serviceResponse.Content = PrStatus.Awaiting;
                }

                if (!pr.Merged && pr.State == ItemState.Closed)
                {
                    serviceResponse.Content = PrStatus.Invalid;
                }

                return serviceResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to aquire data");
                serviceResponse.Content = null;
                serviceResponse.ServiceResponseStatus = ServiceResponseStatus.BadRequest;
                return serviceResponse;

            }
        }

        private async Task<bool> PrIsApproved(string owner, string repo, int id)
        {
            var commits = await _client.PullRequest.Commits(owner, repo, id);
            var reviews = await _client.PullRequest.Review.GetAll(owner, repo, id);


            var latestReview = reviews[0].SubmittedAt;
            var latestCommit = commits[0].Commit.Author.Date;


            if (latestReview > latestCommit)
            {
                return true;
            }
            return false;
        }
    }
}
