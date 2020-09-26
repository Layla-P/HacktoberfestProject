namespace HacktoberfestProject.Web.Models.Entities
{
	public class RepositoryEntity
	{
		public string Owner { get; set; }
		public string Name { get; set; }
		public PrEntity[] PrS { get; set; }
		
		public RepositoryEntity(string owner, string name, PrEntity[] prS)
		{
			Owner = owner;
			Name = name;
			PrS = prS;
		}
	}
}