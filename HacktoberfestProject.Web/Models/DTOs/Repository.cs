using System.Collections.Generic;

namespace HacktoberfestProject.Web.Models.DTOs
{
	public class Repository
	{
		public string Owner { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public List<PullRequest> Prs { get; set; }

		public Repository(string owner, string name, string url = null, List<PullRequest> prs = null)
		{
			Owner = owner;
			Name = name;
			Url = url;
			Prs = prs;
		}

	}
}
