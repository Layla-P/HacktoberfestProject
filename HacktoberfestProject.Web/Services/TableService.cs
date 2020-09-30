using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Data.Repositories;
using System.Linq;
using HacktoberfestProject.Web.Models.DTOs;

namespace HacktoberfestProject.Web.Services
{
    public class TableService : ITableService
    {
        private readonly IUserRepository _userRepository;

        public TableService(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<ServiceResponse<Pr>> AddPrByUsernameAsync(string username, string owner, string repositoryName, Pr pr)
        {
            var user = await GetUser(username);

            Repository repository = GetReporitory(user , owner, repositoryName);

            var serviceResponse = new ServiceResponse<Pr>
            {
                ServiceResponseStatus = ServiceResponseStatus.Created
            };

            if (!repository.Prs.All(p => p.PrId == pr.PrId))
            {
                repository.Prs.Add(pr);
                user.RepositoryPrAddedTo.Add(repository);
                await _userRepository.UpdateAsync(user);
                serviceResponse.Message = "Pr Added!";
            }
            else
            {
                serviceResponse.Message = "Pr was already added!";
                serviceResponse.ServiceResponseStatus = ServiceResponseStatus.Ok;
            }

            serviceResponse.Content = pr;

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsernameAsync(string username)
        {
            var user = await GetUser(username);

            var userEntity = await _userRepository
                                        .ReadAsync(user);

            List<Pr> pr = new List<Pr>();
            var temp = userEntity.RepositoryPrAddedTo;

            foreach (var repository in temp)
            {
                pr.AddRange(repository.Prs);
            }

            var serviceResponse = new ServiceResponse<IEnumerable<Pr>>
            {
                Content = pr,
                ServiceResponseStatus = ServiceResponseStatus.Ok
            };

            return serviceResponse;
        }

        private async Task<User> GetUser(string username)
        {
            User user = new User(username);

            User tablestorageUser = await _userRepository.ReadAsync(user);

            return tablestorageUser;

        }

        private Repository GetReporitory(User user, string owner, string repositoryName)
        {
            Repository repository;

            if (user.RepositoryPrAddedTo.All(r => r.Owner == owner && r.Name == repositoryName))
            {
                repository = user.RepositoryPrAddedTo.FirstOrDefault(r => r.Owner == owner && r.Name == repositoryName);
                user.RepositoryPrAddedTo.Remove(repository);
            }
            else
            {
                repository = new Repository(owner, repositoryName);
            }

            return repository;
        }
    }
}
