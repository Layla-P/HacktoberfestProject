using HacktoberfestProject.Web.Models.Enums;

namespace HacktoberfestProject.Web.Models.Entities
{
	public class PrEntity
	{
		public int PrId { get; set; }
		public string Url { get; set; }
		public PrStatus? Status { get; set; }

		public PrEntity(int prId, string url, PrStatus? status = null)
		{
			PrId = prId;
			Url = url;
			Status = status;
		}
	}
}