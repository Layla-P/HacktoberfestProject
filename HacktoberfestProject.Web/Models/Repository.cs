using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Models
{
	public class Repository
	{
		public string Owner { get; set; }
		public string Name { get; set; }
		public List<Pr> PrEntities { get; set; }

		public Repository(string owner, string name, List<Pr> prEntities = null)
		{
			Owner = owner;
			Name = name;
			PrEntities = prEntities;
		}

	}
}
