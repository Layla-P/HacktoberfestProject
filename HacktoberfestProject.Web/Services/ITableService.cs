using System.Collections.Generic;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Helpers;

namespace HacktoberfestProject.Web.Services
{
	public interface ITableService
    {
        Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsername(string username);

        Task<ServiceResponse<User>> GetUserByUsernameAsync(string username);
    }
}
