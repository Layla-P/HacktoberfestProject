using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using HacktoberfestProject.Web.Data.Configuration;
using HacktoberfestProject.Web.Tools;

namespace HacktoberfestProject.Web.Data
{
	public class TableContext : ITableContext
	{
		private readonly TableConfiguration _configuration;
		private readonly ILogger<TableContext> _logger;

		private CloudTable _table;

		public TableContext(ILogger<TableContext> logger, IOptions<TableConfiguration> tableConfiguration)
		{
			_logger = logger;
			_configuration = tableConfiguration.Value;

			Task.Run(() => CheckForTableAsync());
		}

		public async Task<IEnumerable<T>> GetEntities<T>(string partitionKey) where T : ITableEntity, new()
		{
			if (_table == null) await CheckForTableAsync();
			NullChecker.IsNotNull(partitionKey, nameof(partitionKey));

			try
			{
				TableQuerySegment<T> querySegment = null;
				var entities = new List<T>();
				var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(nameof(ITableEntity.PartitionKey), QueryComparisons.Equal, partitionKey));

				do
				{
					querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
					entities.AddRange(querySegment.Results);
				} while (querySegment.ContinuationToken != null);

				return entities;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Retrieve from table failed");
				throw;
			}
		}

		public async Task<T> InsertOrMergeEntityAsync<T>(T entity) where T : TableEntity
		{
			if (_table == null) await CheckForTableAsync();
			NullChecker.IsNotNull(entity, nameof(entity));

			try
			{
				// Create the InsertOrReplace table operation
				TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

				// Execute the operation.
				TableResult result = await _table.ExecuteAsync(insertOrMergeOperation);

				_logger.LogTrace("Inserting record into table");

				var InsertedEntity = result.Result as T;

				return InsertedEntity;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Insert Or Merge into table failed");
				throw;
			}
		}

		public async Task<T> RetrieveEnitityAsync<T>(T entity) where T : TableEntity
		{
			if (_table == null) await CheckForTableAsync();
			NullChecker.IsNotNull(entity, nameof(entity));

			try
			{
				TableOperation retrieveOperation = TableOperation.Retrieve<T>(entity.PartitionKey, entity.RowKey);

				TableResult result = await _table.ExecuteAsync(retrieveOperation);

				_logger.LogTrace("Retrieving record from table");

				var retrieveEntity = result.Result as T;

				return retrieveEntity;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Retrieve from table failed");
				throw;
			}
		}

		public async Task<bool> DeleteEntity<T>(T entity) where T : TableEntity
		{
			if (_table == null) await CheckForTableAsync();
			NullChecker.IsNotNull(entity, nameof(entity));

			try
			{
				TableOperation deleteOperation = TableOperation.Delete(entity);

				_logger.LogTrace("Deleting record from table");

				TableResult result = await _table.ExecuteAsync(deleteOperation);
				return result.Result != null;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Delete from table failed");
				throw;
			}
		}

		private async Task CheckForTableAsync()
		{
			_logger.LogTrace("Initializing Table Storage.");

			CloudStorageAccount storageAccount = CheckStorageAccount();
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			_table = tableClient.GetTableReference(_configuration.TableName);

			if (await _table.CreateIfNotExistsAsync())
			{
				_logger.LogTrace($"Table {_configuration.TableName} has been created.");
			}
			else
			{
				_logger.LogTrace($"Table {_configuration.TableName} already exsists! Using that table to store data.");
			}
			_logger.LogTrace("Table Storage initialized");
		}

		private CloudStorageAccount CheckStorageAccount()
		{
			try
			{
				return CloudStorageAccount.Parse(_configuration.ConnectionString);
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Invalid storage account connection string. Please check values in appsettings (or user secrets if in dev mode).");
				throw;
			}
		}
	}
}
