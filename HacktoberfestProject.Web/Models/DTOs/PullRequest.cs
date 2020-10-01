namespace HacktoberfestProject.Web.Models.DTOs
{
    public class PullRequest
	{
		public int PrId { get; set; }
		public string Url { get; set; }

		public PullRequest(int prId, string url)
		{
			PrId = prId;
			Url = url;
		}
	}
}
