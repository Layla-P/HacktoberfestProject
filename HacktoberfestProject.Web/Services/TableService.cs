using HacktoberfestProject.Web.Models.Helpers;
using HacktoberfestProject.Web.Models.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HacktoberfestProject.Web.Data.Repositories;
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

        public Task<ServiceResponse<IEnumerable<Pr>>> AddPrByUsernameAsync(string username, Pr pr)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<IEnumerable<Pr>>> GetPrsByUsernameAsync(string username)
        {
            var user = new User(username);

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
    }
}
