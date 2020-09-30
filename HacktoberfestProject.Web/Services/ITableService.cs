using System.Collections.Generic;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsernameAsync(string username);
<<<<<<< HEAD
        Task<ServiceResponse<Pr>> AddPrByUsernameAsync(string username, string owner, string repositoryName, Pr pr);
=======

        Task<ServiceResponse<User>> GetUserByUsernameAsync(string username);

        Task<ServiceResponse<IEnumerable<Pr>>> AddPrByUsernameAsync(string username, Pr pr);
>>>>>>> 2ba122a72ce4925fdff519d646af1cf52495d8de
    }
}
