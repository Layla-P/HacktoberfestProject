using System;
using System.Collections.Generic;
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
                var user = new User(username, new List<Repository>());
                foreach (var entity in entities)
                {
                    Repository repo;
                    bool addToList = false;

                    var info = GetInfo(entity.RowKey);
                    var temp = user.RepositoryPrAddedTo
                                   .FirstOrDefault(repo => repo.Owner.Equals(info.Owner, StringComparison.OrdinalIgnoreCase) &&
                                                           repo.Name.Equals(info.RepoName, StringComparison.OrdinalIgnoreCase));
                    if (temp != null)
                    {
                        repo = temp;
                    }
                    else 
                    {
                        repo = new Repository(info.Owner, info.RepoName, entity.Url, new List<PullRequest>());
                        addToList = true;
                    }
                    var prs = entities.Where(e => e.RowKey.StartsWith($"{info.Owner}:{info.RepoName}", StringComparison.OrdinalIgnoreCase));
                    foreach (var pr in prs)
                    {
                        var id = int.Parse(pr.RowKey.Substring(pr.RowKey.LastIndexOf(':') + 1));
                        if (!repo.Prs.Any(pr => pr.PrId == id))
                        {
                            repo.Prs.Add(new PullRequest(id, entity.Url));
                        }
                    }
                    if (addToList)
                    {
                        user.RepositoryPrAddedTo.Add(repo);
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

        private (string Owner, string RepoName) GetInfo(string rowKey)
        {
            var info = rowKey.Split(':', StringSplitOptions.RemoveEmptyEntries);
            return (info[0], info[1]);
        }
    }
}
