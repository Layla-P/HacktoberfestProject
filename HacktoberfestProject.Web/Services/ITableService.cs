using HacktoberfestProject.Web.Models;
using HacktoberfestProject.Web.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsername(string username);
    }
}
