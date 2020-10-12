using HacktoberfestProject.Web.Models.DTOs;
using Microsoft.Azure.Cosmos.Table;

namespace HacktoberfestProject.Web.Models.Entities
{
	public class ProjectEntity : TableEntity
	{
		public ProjectEntity() { }

		public ProjectEntity(Project project)
		{
			PartitionKey = "Project";
			RowKey = $"{project.Owner}:{project.RepoName}";
			RepoName = project.RepoName;
			Owner = project.Owner;
			Url = project.Url;
		}

		public string RepoName { get; set; }
		public string Url { get; set; }
		public string Owner { get; set; }
	}

	
}
