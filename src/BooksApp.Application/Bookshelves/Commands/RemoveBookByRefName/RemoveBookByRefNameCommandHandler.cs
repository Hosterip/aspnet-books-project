using MediatR;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Bookshelf;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;

internal sealed class RemoveBookByRefNameCommandHandler : IRequestHandler<RemoveBookByRefNameCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBookByRefNameCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBookByRefNameCommand request, CancellationToken cancellationToken)
    {
        var bookshelf = await _unitOfWork.Bookshelves.GetSingleWhereAsync(bookshelf => 
            bookshelf.Name == request.BookshelfRefName &&
            bookshelf.User != null &&
            bookshelf.User.Id == UserId.CreateUserId(request.UserId));
        if (bookshelf is null)
        {
            var user = await _unitOfWork.Users.GetSingleById(request.UserId);
            bookshelf = Bookshelf.Create(user!, request.BookshelfRefName);
            await _unitOfWork.Bookshelves.AddAsync(bookshelf);
        }
        bookshelf!.RemoveBook(request.BookId);
    }
}