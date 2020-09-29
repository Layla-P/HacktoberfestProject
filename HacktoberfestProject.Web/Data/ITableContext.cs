using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Data
{
	public interface ITableContext
	{
		Task<T> InsertOrMergeEntityAsync<T>(T entity) where T : TableEntity;

		Task<T> RetrieveEnitityAsync<T>(T entity) where T : TableEntity;

		Task DeleteEntity<T>(T entity) where T : TableEntity;
	}
}