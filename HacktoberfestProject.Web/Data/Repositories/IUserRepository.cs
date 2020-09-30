using HacktoberfestProject.Web.Models.DTOs;

using System.Threading.Tasks;

namespace HacktoberfestProject.Web.Data.Repositories
{
    public interface IUserRepository
	{
		Task<User> CreateAsync(User user);
		Task<bool> DeleteAsync(User user);
		Task<User> ReadAsync(User user);
		Task<User> UpdateAsync(User user);
	}
}