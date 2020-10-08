using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.Entities;

namespace HacktoberfestProject.Tests.Data
{
	public class TableContextTests
	{
		private ITableContext _context;
		private UserEntity _entityToRemove;

		public TableContextTests(ITableContext context)
		{
			_context = context;
		}

		public void TestInsert()
		{
			PrEntity prEntity = new PrEntity(Constants.PR_ID, Constants.URL);
			RepositoryEntity repositoryEntity = new RepositoryEntity(Constants.TEST_OWNER, Constants.TEST_REPO_NAME, new List<PrEntity>{ prEntity });
			UserEntity userEntity = new UserEntity(Constants.USERNAME, new List<RepositoryEntity> { repositoryEntity });

			var insertedEntity = _context.InsertOrMergeEntityAsync(userEntity).Result;
		}

		public void TestRetrieve()
		{
			UserEntity userEntity = new UserEntity(Constants.USERNAME);

			_entityToRemove = _context.RetrieveEnitityAsync(userEntity).Result;
		}

		public void TestDelete()
		{
			_context.DeleteEntity(_entityToRemove);
		}

		public static void RunTableStorageTests(IServiceCollection services)
		{
			IServiceProvider sp = services.BuildServiceProvider();

			var ctt = new TableContextTests(sp.GetService<ITableContext>());
			ctt.TestInsert();
			ctt.TestRetrieve();
			ctt.TestDelete();
		}
	}
}
