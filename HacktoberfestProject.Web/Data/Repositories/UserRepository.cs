using System;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Entities;

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

			var result = (User)await _tableContext.InsertOrMergeEntityAsync(userEntity);

			return result;
		}

		public async Task<User> ReadAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			var result = (User)await _tableContext.RetrieveEnitityAsync(userEntity);
			
			return result;
		}

		public async Task<User> UpdateAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			var result = (User)await _tableContext.InsertOrMergeEntityAsync(userEntity);

			return result;
		}

		public async Task<bool> DeleteAsync(User user)
		{
			UserEntity userEntity = (UserEntity)user;

			var result = await _tableContext.DeleteEntity(userEntity);

			return result;
		}
	}
}
