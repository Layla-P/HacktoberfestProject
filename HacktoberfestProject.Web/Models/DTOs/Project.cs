using System.ComponentModel.DataAnnotations;
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
		[Display(Name = "Repository Name")]
		public string RepoName { get; set; }
		[Display(Name = "URL")]
		public string Url { get; set; }
		[Display(Name = "Repository Owner")]
		public string Owner { get; set; }
	}
}
