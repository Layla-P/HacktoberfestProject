using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Models;
using HacktoberfestProject.Web.Data.Entities;

namespace HacktoberfestProject.Web.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private ITableContext _tableContext;

		public UserRepository(ITableContext tableContext)
		{
			_tableContext = tableContext ?? throw new ArgumentNullException(nameof(tableContext));
		}

		public async Task<User> CreateAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			return (User)await _tableContext.InsertOrMergeEntityAsync(userEntity);
		}

		public async Task<User> ReadAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			return (User)await _tableContext.RetrieveEnitityAsync(userEntity);
		}

		public async Task<User> UpdateAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			return (User)await _tableContext.InsertOrMergeEntityAsync(userEntity);
		}

		public async Task<bool> DeleteAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;
			return await _tableContext.DeleteEntity(userEntity);
		}
	}
}
