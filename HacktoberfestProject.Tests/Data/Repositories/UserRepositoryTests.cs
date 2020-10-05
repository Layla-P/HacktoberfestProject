using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;
using Xunit;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Data.Repositories;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Entities;

namespace HacktoberfestProject.Tests.Data.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Mock<ITableContext> _tableContext;
        private readonly IUserRepository _sut;
        private readonly UserEntity _userEntity;
        private readonly PrEntity _prEntity;
        private readonly User _user;

        public UserRepositoryTests()
        {
            _prEntity = new PrEntity(Constants.PR_ID, Constants.URL);
            RepositoryEntity repository = new RepositoryEntity(Constants.TEST_OWNER, Constants.TEST_REPO_NAME, new List<PrEntity> { _prEntity });
            _userEntity = new UserEntity(Constants.OWNER, new List<RepositoryEntity> { repository });

            _user = (User)_userEntity;

            _tableContext = new Mock<ITableContext>(MockBehavior.Strict);
            _sut = new UserRepository(_tableContext.Object);
        }

        [Fact]
        public void GivenNullTableContext_should_ThrowException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new UserRepository(null));
            Assert.Equal("Value cannot be null. (Parameter 'tableContext')", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_GivenUser_ShouldReturn_User()
        {
            _tableContext.Setup(e => e.InsertOrMergeEntityAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(_userEntity);

            var result = await _sut.CreateAsync(_user);

            Assert.Equal(result.Username, Constants.OWNER);
            Assert.Equal(result.RepositoryPrAddedTo.FirstOrDefault().Name, _user.RepositoryPrAddedTo.FirstOrDefault().Name);
        }

        [Fact]
        public async Task ReadAsync_GivenUser_ShouldReturn_User()
        {
            _tableContext.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(_userEntity);

            var result = await _sut.ReadAsync(new User(Constants.OWNER));

            Assert.Equal(result.Username, Constants.OWNER);
        }

        [Fact]
        public async Task UpdateAsync_GivenUser_ShouldReturn_User()
        {
            _tableContext.Setup(e => e.InsertOrMergeEntityAsync(It.IsAny<UserEntity>()))
                .ReturnsAsync(_userEntity);

            _user.RepositoryPrAddedTo.FirstOrDefault().Name = "Changed";
            var result = await _sut.UpdateAsync(_user);

            Assert.Equal(result.Username, Constants.OWNER);
        }

        [Fact]
        public async Task DeleteAsync_GivenUser_ShouldReturn_User()
        {
            _tableContext.Setup(e => e.DeleteEntity(It.IsAny<UserEntity>()))
                .ReturnsAsync(true);

            var result = await _sut.DeleteAsync(new User(Constants.OWNER));

            Assert.True(result);
        }
    }
}
