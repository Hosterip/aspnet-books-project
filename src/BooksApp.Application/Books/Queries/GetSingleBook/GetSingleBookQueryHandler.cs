using MediatR;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Queries.GetSingleBook;

internal sealed class GetSingleBookQueryHandler : IRequestHandler<GetSingleBookQuery, BookResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSingleBookQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<BookResult> Handle(GetSingleBookQuery request, CancellationToken cancellationToken)
    {
        var post = await _unitOfWork.Books.GetSingleWhereAsync(post => post.Id == request.Id);

        var user = new UserResult
        {
            Id = post!.Author.Id, 
            Username = post!.Author.Username,
            Role = post.Author.Role.Name
        };
        var likeCount = await _unitOfWork.Likes.CountLikes(request.Id);
        
        return new BookResult { Id = post.Id, Title = post.Title, Description = post.Description, Author = user, LikeCount = likeCount};
    }
}