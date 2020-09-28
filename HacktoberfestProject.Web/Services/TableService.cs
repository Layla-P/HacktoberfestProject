using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Services
{
    public class TableService : ITableService
    {
        private readonly ITableContext _tableContext;

        public TableService(ITableContext tableContext)
        {
            _tableContext = tableContext ?? throw new ArgumentNullException(nameof(tableContext));
        }

        public async Task<ServiceResponse<IEnumerable<PrEntity>>> GetPrsByUsername(string username)
        {
            var user = new UserEntity(username);

            var userEntity = await _tableContext
                                        .RetrieveEnitityAsync(user);

            List<PrEntity> prEntities = new List<PrEntity>();
            var temp = userEntity.RepositoryPrAddedTo;

            foreach (var prEntity in temp)
            {
                prEntities.AddRange(prEntity.PrEntities);
            }

            var serviceResponse = new ServiceResponse<IEnumerable<PrEntity>>
            {
                Content = prEntities,
                ServiceResponseStatus = ServiceResponseStatus.Ok
            };

            return serviceResponse;
        }
    }
}
