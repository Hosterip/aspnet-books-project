using PostsApp.Application.Common.Results;
using PostsApp.Domain.Models;

namespace PostsApp.Application.Common.Interfaces.Repositories;

public interface IUsersRepository : IGenericRepository<User>
{
    Task<PaginatedArray<UserResult>> GetPaginated(int page, int limit, string query);
}