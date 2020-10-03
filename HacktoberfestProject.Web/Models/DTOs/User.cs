using System.Collections.Generic;

namespace HacktoberfestProject.Web.Models.DTOs
{
    public class User
	{
		public string Username { get; set; }

		public List<Repository> RepositoryPrAddedTo { get; set; }

		public User(string username, List<Repository> repositoryPrAddedTo = null)
		{
			RepositoryPrAddedTo = repositoryPrAddedTo ?? new List<Repository>();
			Username = username;
		}
	}
}
