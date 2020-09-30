namespace HacktoberfestProject.Web.Models.DTOs
{
    public class Pr
	{
		public int PrId { get; set; }
		public string Url { get; set; }

		public Pr(int prId, string url)
		{
			PrId = prId;
			Url = url;
		}
	}
}
