using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<PullRequest>> AddPrAsync(string username, string owner, string repositoryName, PullRequest pr);

        Task<ServiceResponse<User>> GetUserAsync(string username);
    }
}
