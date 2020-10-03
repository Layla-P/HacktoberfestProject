using HacktoberfestProject.Web.Models.DTOs;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
    public interface IGithubService
	{
		Task<List<PullRequest>> GetPullRequestsForRepo(string owner, string name);
		Task<List<Repository>> GetRepos(string owner);
	}
}