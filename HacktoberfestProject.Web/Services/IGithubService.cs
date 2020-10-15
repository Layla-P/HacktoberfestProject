using System.Collections.Generic;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
	public interface IGithubService
	{
		Task<List<Contributor>> GetContributorsAsync();
		Task<List<PullRequest>> GetPullRequestsForRepo(string owner, string name);
		Task<List<Repository>> GetRepos(string owner);
		Task<ServiceResponse<IEnumerable<TypeaheadResult>>> SearchOwners(string owner, int limit);
		Task<ServiceResponse<PrStatus?>> ValidatePrStatus(string owner, string repo, int id);
	}
}
