using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsernameAsync(string username);
        Task<ServiceResponse<Pr>> AddPrByUsernameAsync(string username, string owner, string repositoryName, Pr pr);
    }
}
