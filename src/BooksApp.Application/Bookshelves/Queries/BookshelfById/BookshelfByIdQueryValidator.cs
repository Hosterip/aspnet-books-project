using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfById;

internal sealed class BookshelfByNameQueryValidator : AbstractValidator<BookshelfByIdQuery>
{
    public BookshelfByNameQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) => 
                await unitOfWork.Bookshelves.AnyById(bookshelfId))
            .WithMessage(BookshelfValidationMessages.NotFound);
    }
}