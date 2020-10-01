using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<Pr>> AddPrAsync(string username, string owner, string repositoryName, Pr pr);

        Task<ServiceResponse<User>> GetUserAsync(string username);
    }
}
