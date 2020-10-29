using System;
using Microsoft.Extensions.DependencyInjection;
using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.Entities;

namespace HacktoberfestProject.Tests.Data
{
	public class TableContextTests
	{
		private readonly ITableContext _context;
		private TrackerEntryEntity _entityToRemove;
		private readonly TrackerEntryEntity _trackerEntryEntity;

		public TableContextTests(ITableContext context)
		{
			_context = context;

			_trackerEntryEntity = new TrackerEntryEntity
			{
				Username = Constants.USERNAME,
				Url = Constants.URL,
				Status = null
			};
		}

		public void TestInsert()
		{			
			_ = _context.InsertOrMergeEntityAsync(_trackerEntryEntity).Result;
		}

		public void TestRetrieve()
		{
			_entityToRemove = _context.RetrieveEnitityAsync(_trackerEntryEntity).Result;
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
