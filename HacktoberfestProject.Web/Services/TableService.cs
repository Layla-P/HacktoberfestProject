using System;
using System.Linq;
using System.Threading.Tasks;

using HacktoberfestProject.Web.Data;
using HacktoberfestProject.Web.Models.DTOs;
using HacktoberfestProject.Web.Models.Entities;
using HacktoberfestProject.Web.Models.Enums;
using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Tools;

namespace HacktoberfestProject.Web.Services
{
    public class TableService : ITableService
    {
        private readonly ITableContext _tableContext;

        public TableService(ITableContext tableContext)
        {
            NullChecker.IsNotNull(tableContext, nameof(tableContext));
            _tableContext = tableContext;
        }

        public async Task<ServiceResponse<PullRequest>> AddPrAsync(string username, string owner, string repositoryName, PullRequest pr)
        {
            var trackerEntry = new TrackerEntryEntity
            { 
                Username = username,
                RowKey = $"{owner}:{repositoryName}:{pr.PrId}",
                Url = pr.Url
            };
            await _tableContext.InsertOrMergeEntityAsync(trackerEntry);
            var serviceResponse = new ServiceResponse<PullRequest>
            {
                ServiceResponseStatus = ServiceResponseStatus.Created
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<User>> GetUserAsync(string username)
        {
            var entities = await _tableContext.GetEntities<TrackerEntryEntity>(username);
            var serviceResponse = new ServiceResponse<User>
            {
                ServiceResponseStatus = ServiceResponseStatus.NotFound
            };

            if (entities.Any())
            {
                var user = new User(username);
                foreach (var entity in entities.OrderBy(e => e.RowKey))
                {
                    var info = entity.RowKey.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length != 3)
                    {
                        // TODO: define how to proceed with this
                        continue;
                    }
                    var repo = new Repository(info[0], info[1], entity.Url);
                    var prs = entities.Where(e => e.RowKey.StartsWith($"{info[0]}:{info[1]}"));
                    foreach (var pr in prs)
                    {
                        repo.Prs.Add(new PullRequest(int.Parse(info[2]), entity.Url));
                    }
                }

                serviceResponse = new ServiceResponse<User>
                {
                    Content = user,
                    ServiceResponseStatus = ServiceResponseStatus.Ok
                };
            }

            return serviceResponse;
        }
    }
}
