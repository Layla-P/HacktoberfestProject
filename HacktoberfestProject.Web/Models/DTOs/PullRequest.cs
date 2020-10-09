using HacktoberfestProject.Web.Models.Enums;

namespace HacktoberfestProject.Web.Models.DTOs
{
	public class PullRequest
	{
		public int PrId { get; set; }
		public string Url { get; set; }
		public PrStatus? Status { get; set; }

		public PullRequest(int prId, string url, PrStatus? status = null)
		{
			PrId = prId;
			Url = url;
			Status = status;
		}
	}
}
