using HacktoberfestProject.Web.Models.Entities;

namespace HacktoberfestProject.Web.Models.DTOs
{
	public class Project
	{
		public Project() { }
		public Project(ProjectEntity projectEntity)
		{
			RepoName = projectEntity.RepoName;
			Owner = projectEntity.Owner;
			Url = projectEntity.Url;
		}
		public string RepoName { get; set; }
		public string Url { get; set; }
		public string Owner { get; set; }
	}
}
