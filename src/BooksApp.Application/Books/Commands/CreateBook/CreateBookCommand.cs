using MediatR;
using PostsApp.Application.Books.Results;

namespace PostsApp.Application.Books.Commands.CreateBook;

public sealed class CreateBookCommand : IRequest<BookResult>
{
    public required int UserId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
}