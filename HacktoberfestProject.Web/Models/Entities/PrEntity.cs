﻿namespace HacktoberfestProject.Web.Models.Entities
{
	public class PrEntity
	{
		public int PrId { get; set; }

		public string Title { get; set; }

		public string Url { get; set; }

		public PrEntity(int prId, string url)
		{
			PrId = prId;
			Url = url;
		}
	}
}