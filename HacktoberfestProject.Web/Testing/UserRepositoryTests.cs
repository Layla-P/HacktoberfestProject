using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Data.Repositories;
using HacktoberfestProject.Web.Models.DTOs;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Testing
{
	[TestFixture]
	public class UserRepositoryTests
	{
		Mock<ITableContext> _tableContext;
		IUserRepository _sut;

		private string _username;
		private UserEntity _userEntity;
		private PrEntity _prEntity;
		private User _user;

		[SetUp]
		public void Setup()
		{
			_username = "Layla-P";
			_prEntity = new PrEntity(3, "http://test");
			RepositoryEntity repository = new RepositoryEntity("test", "test", new List<PrEntity> { _prEntity });
			_userEntity = new UserEntity(_username, new List<RepositoryEntity> { repository });

			_user = (User)_userEntity;

			_tableContext = new Mock<ITableContext>(MockBehavior.Strict);
			_sut = new UserRepository(_tableContext.Object);
		}

		[Test]
		public void GivenNullTableContext_should_ThrowException()
		{
			Assert.That(
				() => new UserRepository(null),
				Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'tableContext')"));
		}

		[Test]
		public async Task CreateAsync_GivenUser_ShouldReturn_User()
		{
			_tableContext.Setup(e => e.InsertOrMergeEntityAsync(It.IsAny<UserEntity>()))
				.ReturnsAsync(_userEntity);

			var result = await _sut.CreateAsync(_user);

			Assert.That(result.Username, Is.EqualTo(_username));
			Assert.That(result.RepositoryPrAddedTo.FirstOrDefault().Name, 
						Is.EqualTo(_user.RepositoryPrAddedTo.FirstOrDefault().Name));
		}

		[Test]
		public async Task ReadAsync_GivenUser_ShouldReturn_User()
		{
			_tableContext.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
				.ReturnsAsync(_userEntity);

			var result = await _sut.ReadAsync(new User(_username));

			Assert.That(result.Username, Is.EqualTo(_username));
		}

		[Test]
		public async Task UpdateAsync_GivenUser_ShouldReturn_User()
		{
			_tableContext.Setup(e => e.InsertOrMergeEntityAsync(It.IsAny<UserEntity>()))
				.ReturnsAsync(_userEntity);

			_user.RepositoryPrAddedTo.FirstOrDefault().Name = "Changed";
			var result = await _sut.UpdateAsync(_user);

			Assert.That(result.Username, Is.EqualTo(_username));
		}

		[Test]
		public async Task DeleteAsync_GivenUser_ShouldReturn_User()
		{
			_tableContext.Setup(e => e.DeleteEntity(It.IsAny<UserEntity>()))
				.ReturnsAsync(true);

			var result = await _sut.DeleteAsync(new User(_username));

			Assert.That(result, Is.True);
		}
	}
}
