using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Services;
using HacktoberfestProject.Web.Tools;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HacktoberfestProject.Web.Controllers
{
	public class ProjectsController : Controller
	{
		private readonly IProjectService _projectService;
		public ProjectsController(
			IProjectService projectService)
		{
			NullChecker.IsNotNull(projectService, nameof(projectService));
			_projectService = projectService;
		}
		public async Task<IActionResult> Index()
		{
			var result = await _projectService.GetAllProjectsAsync();			
			return View(result.Content);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Add(Project project)
		{
			if (!ModelState.IsValid)
			{
				return View("Add", project);
			}

			var result = await _projectService
				.AddProjectAsync(project);

			return RedirectToAction("Index", "Projects");
		}
	
	}
}
