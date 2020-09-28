using HacktoberfestProject.Web.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
	public class GithubService : IGithubService
	{
		private ILogger<GithubService> _logger;
		private GitHubClient _client = new GitHubClient(new ProductHeaderValue("HacktoberfestProject"));

		public GithubService(ILogger<GithubService> logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<List<Models.Repository>> GetRepos(string owner)
		{
			_logger.LogTrace($"Sending request to Github for repositories belonging to user: {owner}");
			var repositories = await _client.Repository.GetAllForUser(owner);

			return repositories.Select(r => new Models.Repository(owner, r.Name, r.Url)).ToList();
		}

		public async Task<List<Pr>> GetPullRequestsForRepo(string owner, string name)
		{
			_logger.LogTrace($"Sending request to Github for pull requests on repositoy: {name}");
			var prs = await _client.PullRequest.GetAllForRepository(owner, name, new PullRequestRequest() { State = ItemStateFilter.All});

			return prs.Select(pr => new Pr(pr.Number, pr.Url)).ToList();
		}
	}
}
