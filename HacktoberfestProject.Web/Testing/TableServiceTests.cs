using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Data.Configuration;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Testing
{
    [TestFixture]
    public class TableServiceTests
    {
        Mock<ITableContext> tableContextMock;
        ITableService sut;


        private string username;
        private UserEntity userEntity;
        private PrEntity prEntity;
        private ServiceResponse<IEnumerable<PrEntity>> expectedResult;

        [SetUp]
        public void Setup()
        {
            username = "Layla-P";
             prEntity = new PrEntity(3, "http://test");
            RepositoryEntity repositoryEntity = new RepositoryEntity("test", "test", new List<PrEntity> { prEntity });
            userEntity = new UserEntity(username, new List<RepositoryEntity> { repositoryEntity });
            expectedResult = new ServiceResponse<IEnumerable<PrEntity>>
            {
                Content = new List<PrEntity> { prEntity },
                ServiceResponseStatus = ServiceResponseStatus.Ok,
                Message = "test message"
            };


            tableContextMock = new Mock<ITableContext>(MockBehavior.Strict);
            sut = new TableService(tableContextMock.Object);
        }


        [Test]
        public void GivenNullTableContext_Should_ThrowException()
        {
            Assert.That(
                () => new TableService(null),
                Throws.ArgumentNullException.With.Message.EqualTo("Value cannot be null. (Parameter 'tableContext')"));
        }

        [Test]
        public async Task GivenUsername_ShouldReturn_ServiceResponse()
        {
            tableContextMock.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
               .ReturnsAsync(userEntity);

            var result = await sut.GetPrsByUsername(username);

            Assert.That(result.Content, Is.EqualTo(new List<PrEntity> { prEntity }));
            Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
        }

        //[Test]
        //public async Task GivenUsernameAndPR_ShouldAddPR_ServiceResponse()
        //{
        //    tableContextMock.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
        //       .ReturnsAsync(userEntity);

        //    var result = await sut.GetPrsByUsername(username);

        //    Assert.That(result.Content, Is.EqualTo(new List<PrEntity> { prEntity }));
        //    Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
        //}

    }
}
