using HacktoberfestProject.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
	public interface IGithubService
	{
		Task<List<Pr>> GetPullRequestsForRepo(string owner, string name);
		Task<List<Repository>> GetRepos(string owner);
	}
}