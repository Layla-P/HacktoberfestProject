using System.Collections.Generic;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
	public interface IProjectService
	{
		Task<ServiceResponse<IEnumerable<Project>>> GetAllProjectsAsync();
		Task<ServiceResponse> AddProjectAsync(Project project);
	}
}
