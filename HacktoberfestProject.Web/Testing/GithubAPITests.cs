using HacktoberfestProject.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Testing
{
	public class GithubAPITests
	{

		public static async void RunTableStorageTests(IServiceCollection services)
		{
			IServiceProvider sp = services.BuildServiceProvider();

			var githubservice = sp.GetService<IGithubService>();

			var repos = await githubservice.GetRepos("Layla-p");

			
			var prs = await  githubservice.GetPullRequestsForRepo("Layla-p", "HacktoberfestProject"); 

			//place breakpoint here and check that repos and prs contains values.
		}
	}
}
