using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;

namespace HacktoberfestProject.Web.Data
{
	public interface ITableContext
	{
		Task<T> InsertOrMergeEntityAsync<T>(T entity) where T : TableEntity;

		Task<T> RetrieveEnitityAsync<T>(T entity) where T : TableEntity;

		Task<bool> DeleteEntity<T>(T entity) where T : TableEntity;

		Task<IEnumerable<T>> GetEntities<T>(string partitionKey) where T : ITableEntity, new();
	}
}