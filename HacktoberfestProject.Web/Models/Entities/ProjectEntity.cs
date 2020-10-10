using Microsoft.Azure.Cosmos.Table;

namespace HacktoberfestProject.Web.Models.Entities
{
	public class ProjectEntity : TableEntity
	{
		[IgnoreProperty] 
		public string RepoName 
		{
			get => PartitionKey;
			set => PartitionKey = value;
		}
		public string Url { get; set; }
		public string Owner { get; set; }
	}
}
