using System.Collections.Generic;

namespace HacktoberfestProject.Web.Models.Entities
{
    public class RepositoryEntity
    {
        public string Owner { get; set; }
        public string Name { get; set; }
        public List<PrEntity> PrEntities { get; set; }
        
        public RepositoryEntity(string owner, string name, List<PrEntity> prEntities=null)
        {
            Owner = owner;
            Name = name;
            PrEntities = prEntities;
        }
    }
}