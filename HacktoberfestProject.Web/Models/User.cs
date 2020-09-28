using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Models
{
	public class User
	{
		public string Username { get; set; }
		public List<Repository> RepositoryPrAddedTo { get; set; }

		public User(string username, List<Repository> repositoryPrAddedTo = null)
		{
			RepositoryPrAddedTo = repositoryPrAddedTo;
			Username = username;
		}
	}
}
