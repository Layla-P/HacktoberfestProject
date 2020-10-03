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
                    bool add = false;
                    var info = entity.RowKey.Split(':', StringSplitOptions.RemoveEmptyEntries);
                    if (info.Length != 3)
                    {
                        // TODO: define how to proceed with this.. this is corrupted data!
                        continue;
                    }
                    var owner = info[0];
                    var repoName = info[1];
                    var prId = int.Parse(info[2]);

                    Repository repo;
                    var temp = user.RepositoryPrAddedTo
                                   .FirstOrDefault(repo => repo.Owner.Equals(owner, StringComparison.OrdinalIgnoreCase) &&
                                                           repo.Name.Equals(repoName, StringComparison.OrdinalIgnoreCase));
                    if (temp != null)
                    {
                        repo = temp;
                    }
                    else 
                    {
                        repo = new Repository(owner, repoName, entity.Url, new List<PullRequest>());
                        add = true;
                    }
                    var prs = entities.Where(e => e.RowKey.StartsWith($"{owner}:{repoName}", StringComparison.OrdinalIgnoreCase));
                    foreach (var pr in prs)
                    {
                        var id = int.Parse(pr.RowKey.Substring(pr.RowKey.LastIndexOf(':') + 1));
                        if (!repo.Prs.Any(pr => pr.PrId == id))
                        {
                            repo.Prs.Add(new PullRequest(id, entity.Url));
                        }
                    }
                    if (add)
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
    }
}
