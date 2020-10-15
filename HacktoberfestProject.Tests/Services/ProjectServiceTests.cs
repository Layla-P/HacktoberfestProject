using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Services;
using Moq;
using Xunit;

namespace HacktoberfestProject.Tests.Services
{
	public class ProjectServiceTests
	{
		Project _project;
		ProjectEntity _projectEntity;
		IEnumerable<ProjectEntity> _projectEntities;
		IEnumerable<Project> _projects;
		readonly Mock<ITableContext> _tableContextMock;
		readonly IProjectService _sut;

		public ProjectServiceTests()
		{
			_project = new Project
			{
				RepoName = Constants.REPO_NAME,
				Owner = Constants.OWNER,
				Url = Constants.URL
			};
			_projectEntity = new ProjectEntity(_project);
			_projects = new List<Project>();

			_tableContextMock = new Mock<ITableContext>(MockBehavior.Strict);
			_sut = new ProjectService(_tableContextMock.Object);
		}


		[Fact]
		public void GivenNullUserRepository_Should_ThrowException()
		{
			var ex = Assert.Throws<ArgumentNullException>(() => new ProjectService(null));
			Assert.Equal("Value cannot be null. (Parameter 'tableContext')", ex.Message);
		}

		[Fact]
		public async Task GivenProject_ShouldSaveAndReturn_ServiceResponse()
		{
			_tableContextMock.Setup(e => e.InsertOrMergeEntityAsync(It.IsAny<ProjectEntity>()))
			   .ReturnsAsync(_projectEntity);
			var result = await _sut.AddProjectAsync(_project);
			var expectedResult = new ServiceResponse
			{
				ServiceResponseStatus = ServiceResponseStatus.Ok,
				Message = $"The project {Constants.REPO_NAME} was added."
			};
			result.Should().BeEquivalentTo(expectedResult);
		}

		[Fact]
		public async Task GetAllProjects_ShouldReturn_IEnumerableOfProjects()
		{
			_projectEntities = new List<ProjectEntity>();
			_tableContextMock.Setup(e => e.GetEntities<ProjectEntity>(It.IsAny<string>()))
			   .ReturnsAsync(_projectEntities);


			var result = await _sut.GetAllProjectsAsync();

			var response = new ServiceResponse<IEnumerable<Project>>
			{
				Content = _projects,
				ServiceResponseStatus = ServiceResponseStatus.Ok,
				Message = "Successful Retrieval"
			};
			result.Should().BeEquivalentTo(response);

		}
	}
}
