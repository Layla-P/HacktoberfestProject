using HacktoberfestProject.Web.Data.Repositories;
using HacktoberfestProject.Web.Models;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Testing
{
    [TestFixture]
    public class TableServiceTests
    {
        Mock<IUserRepository> _userRepository;
        ITableService _sut;


        private string _username;
        private User _user;
        private Pr _pr;
        private ServiceResponse<IEnumerable<Pr>> _expectedResult;

        [SetUp]
        public void Setup()
        {
            _username = "Layla-P";
            _pr = new Pr(3, "http://test");
            Repository repository = new Repository("test", "test", new List<Pr> { _pr });
            _user = new User(_username, new List<Repository> { repository });
            _expectedResult = new ServiceResponse<IEnumerable<Pr>>
            {
                Content = new List<Pr> { _pr },
                ServiceResponseStatus = ServiceResponseStatus.Ok,
                Message = "test message"
            };


            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _sut = new TableService(_userRepository.Object);
        }


        [Test]
        public void GivenNullUserRepository_Should_ThrowException()
        {
            Assert.That(
                () => new TableService(null),
                Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'userRepository')"));
        }

        [Test]
        public async Task GivenUsername_ShouldReturn_ServiceResponse()
        {
            _userRepository.Setup(e => e.ReadAsync(It.IsAny<User>()))
               .ReturnsAsync(_user);

            var result = await _sut.GetPrsByUsername(_username);

            Assert.That(result.Content, Is.EqualTo(new List<Pr> { _pr }));
            Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
        }

        //[Test]
        //public async Task GivenUsernameAndPR_ShouldAddPR_ServiceResponse()
        //{
        //	tableContextMock.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
        //	   .ReturnsAsync(userEntity);

        //	var result = await sut.GetPrsByUsername(username);

        //	Assert.That(result.Content, Is.EqualTo(new List<PrEntity> { prEntity }));
        //	Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
        //}

    }
}
