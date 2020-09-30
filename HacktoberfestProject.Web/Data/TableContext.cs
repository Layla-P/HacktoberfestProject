using System;
using System.Threading.Tasks;

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using HacktoberfestProject.Web.Data.Configuration;

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
				_logger.LogTrace($"Table {_configuration.TableName} already exsists! using that table to stored data.");
			}
			_logger.LogTrace("Table Storage initialized");
		}

		private CloudStorageAccount CheckStorageAccount()
		{
			CloudStorageAccount storageAccount;
			
			try
			{
				storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Invaild storage account connection string. Please check values in the appsettings file(or user secrets if in dev mode).");
				throw;
			}

			return storageAccount;
		}

		public async Task<T> InsertOrMergeEntityAsync<T>(T entity) where T : TableEntity
		{
			if (_table == null) await CheckForTableAsync();

			if (entity == null) throw new ArgumentNullException(nameof(entity));

			try
			{
				// Create the InsertOrReplace table operation
				TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

				// Execute the operation.
				TableResult result = await _table.ExecuteAsync(insertOrMergeOperation);

				_logger.LogTrace("Inserting record to table");

				var InsertedEntity = result.Result as T;

				return InsertedEntity;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Insert Or Merge to table failed");
				throw;
			}
		}

		public async Task<T> RetrieveEnitityAsync<T>(T userEntity) where T : TableEntity
		{
			if (_table == null) await CheckForTableAsync();

			if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));

			try
			{
				TableOperation retriveOperation = TableOperation.Retrieve<T>(userEntity.PartitionKey, userEntity.RowKey);

				TableResult result = await _table.ExecuteAsync(retriveOperation);

				_logger.LogTrace("Retriving record from table");
				
				var retriveEntity =	result.Result as T;

				return retriveEntity;
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Retrive from table failed");
				throw;
			}
		}

		public async Task<bool> DeleteEntity<T>(T userEntity) where T: TableEntity
		{
			if (_table == null) await CheckForTableAsync();

			if (userEntity == null) throw new ArgumentNullException(nameof(userEntity));

			try
			{
				TableOperation deleteOperation = TableOperation.Delete(userEntity);

				_logger.LogTrace("Deleteing Record");

				TableResult result = await _table.ExecuteAsync(deleteOperation);
				if (result.Result != null)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception e)
			{
				_logger.LogError(e.HResult, e, "Remove from table failed");
				throw;
			}
		}
	}
}
