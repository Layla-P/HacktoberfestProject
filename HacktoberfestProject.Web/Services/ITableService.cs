using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
    public interface ITableService
    {
        Task<ServiceResponse<IEnumerable<PrEntity>>> GetPrsByUsername(string username);
    }
}
