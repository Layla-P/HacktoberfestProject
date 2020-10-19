using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Tools;

namespace HacktoberfestProject.Web.Services
{
	public class ProjectService : IProjectService
	{
		private readonly ITableContext _tableContext;
		public ProjectService(ITableContext tableContext)
		{
			NullChecker.IsNotNull(tableContext, nameof(tableContext));
			_tableContext = tableContext;
			
		}
		public async Task<ServiceResponse> AddProjectAsync(Project project)
		{
			var projectEntity = new ProjectEntity(project);

			await _tableContext.InsertOrMergeEntityAsync(projectEntity);
			//todo: add in error handling
			return new ServiceResponse
			{
				ServiceResponseStatus = ServiceResponseStatus.Ok,
				Message = $"The project {project.RepoName} was added."
			};
		}
		public async Task<ServiceResponse<IEnumerable<Project>>> GetAllProjectsAsync()
		{
			var projectEntities = await _tableContext.GetEntities<ProjectEntity>("Project");

			//todo: add in error handling
			var response = new ServiceResponse<IEnumerable<Project>>
			{
				Content = projectEntities.Select(x => new Project(x)),
				ServiceResponseStatus = ServiceResponseStatus.Ok,
				Message = "Successful Retrieval"
			};

			return response;
		}
	}
}
