using MediatR;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Users.Results;

namespace PostsApp.Application.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PaginatedArray<UserResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PaginatedArray<UserResult>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        string query = request.Query ?? "";
        int limit = request.Limit ?? 10;
        int page = request.Page ?? 1;

        var result = await _unitOfWork.Users.GetPaginated(page, limit, query);
        return result;
    }
}