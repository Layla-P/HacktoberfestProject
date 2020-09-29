namespace HacktoberfestProject.Web.Models.Entities
{
	public class RepositoryEntity
	{
		public string Owner { get; set; }
		public string Name { get; set; }
		public PrEntity[] PrEntities { get; set; }
		
		public RepositoryEntity(string owner, string name, PrEntity[] prEntities)
		{
			Owner = owner;
			Name = name;
			PrEntities = prEntities;
		}
	}
}