using System;
using System.Collections.Generic;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Services;

using Moq;

using Xunit;

namespace HacktoberfestProject.Tests.Services
{
	public class TrackerEntryServiceTests
	{
		readonly Mock<ITableContext> _tableContext;
		readonly Mock<IGithubService> _githubService;
		readonly ITrackerEntryService _sut;

		private readonly User _user;
		private readonly PullRequest _pr;
		private readonly ServiceResponse<IEnumerable<PullRequest>> _expectedResult;

		public TrackerEntryServiceTests()
		{
			_pr = new PullRequest(Constants.PR_ID, Constants.URL);
			Repository repository = new Repository(Constants.OWNER, Constants.REPO_NAME, null, new List<PullRequest> { _pr });
			_user = new User(Constants.USERNAME, new List<Repository> { repository });
			_expectedResult = new ServiceResponse<IEnumerable<PullRequest>>
			{
				Content = new List<PullRequest> { _pr },
				ServiceResponseStatus = ServiceResponseStatus.Ok,
				Message = Constants.MESSAGE
			};

			_tableContext = new Mock<ITableContext>(MockBehavior.Strict);
			_githubService = new Mock<IGithubService>(MockBehavior.Strict);
			_sut = new TrackerEntryService(_tableContext.Object, _githubService.Object);
		}


		[Fact]
		public void GivenNullUserRepository_Should_ThrowException()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new TrackerEntryService(null, _githubService.Object));
			Assert.Equal("Value cannot be null. (Parameter 'tableContext')", ex.Message);
		}

		[Fact]
		public void GivenNullGithubService_Should_ThrowException()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new TrackerEntryService(_tableContext.Object, null));
			Assert.Equal("Value cannot be null. (Parameter 'githubService')", ex.Message);
		}


		//[Fact]
		//public async Task GivenUsername_ShouldReturn_ServiceResponse()
		//{
		//	_tableContext.Setup(e => e.ReadAsync(It.IsAny<User>()))
		//	   .ReturnsAsync(_user);

		//	var result = await _sut.GetPrsByUsernameAsync(_username);

		//	Assert.That(result.Content, Is.EqualTo(new List<Pr> { _pr }));
		//	Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
		//}

		//[Fact]
		//public async Task GivenUsernameAndPR_ShouldAddPR_ServiceResponse()
		//{
		//	tableContextMock.Setup(e => e.RetrieveEnitityAsync(It.IsAny<UserEntity>()))
		//	   .ReturnsAsync(userEntity);

		//	var result = await sut.GetPrsByUsername(username);

		//	Assert.That(result.Content, Is.EqualTo(new List<PrEntity> { prEntity }));
		//	Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Ok));
		//}

		//[Fact]
		//public async Task GivenUsernameAndOwnerAndRepositoynameAndUniquePr_ShouldReturn_ServiceResponseCreated()
		//{
		//	_tableContext.Setup(e => e.ReadAsync(It.IsAny<User>()))
		//		.ReturnsAsync(_user);

		//	_tableContext.Setup(e => e.UpdateAsync(It.IsAny<User>()))
		//		.ReturnsAsync(_user);

		//	var pr = new Pr(100, "fakeURL");

		//	var result = await _sut.AddPrByUsernameAsync(_username, _owner, _repositoryName, pr);

		//	Assert.That(result.Content, Is.EqualTo(pr));
		//	Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.Created));
		//}

		//[Fact]
		//public async Task GivenUsernameAndOwnerAndRepositoynameAndDuplicatePr_ShouldReturn_ServiceResponseDuplicateFound()
		//{
		//	_tableContext.Setup(e => e.ReadAsync(It.IsAny<User>()))
		//		.ReturnsAsync(_user);

		//	_tableContext.Setup(e => e.UpdateAsync(It.IsAny<User>()))
		//		.ReturnsAsync(_user);



		//	var result = await _sut.AddPrByUsernameAsync(_username, _owner, _repositoryName, _pr);

		//	Assert.That(result.Content, Is.EqualTo(_pr));
		//	Assert.That(result.ServiceResponseStatus, Is.EqualTo(ServiceResponseStatus.DuplicateFound));
		//}
	}
}
