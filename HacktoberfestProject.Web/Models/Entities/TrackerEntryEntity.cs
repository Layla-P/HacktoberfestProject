using HacktoberfestProject.Web.Models.Enums;
using Microsoft.Azure.Cosmos.Table;

namespace HacktoberfestProject.Web.Models.Entities
{
	public class TrackerEntryEntity : TableEntity
	{
		[IgnoreProperty]
		public string Username
		{
			get => PartitionKey;
			set => PartitionKey = value;
		}

		public string Url { get; set; }

		public PrStatus? Status { get; set; }
	}
}
