using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Extensions;
using PostsApp.Application.Common.Interfaces.Repositories;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Book;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Bookshelf.ValueObjects;
using PostsApp.Domain.Genre;
using PostsApp.Infrastructure.Data;

namespace PostsApp.Infrastructure.Implementation.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    public BooksRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedArray<BookResult>> GetPaginated(int limit, int page,
        Expression<Func<Book, bool>> expression)
    {
        var result = await
            (
                from book in _dbContext.Books
                    .Where(expression)
                let user = new UserResult
                {
                    Id = book.Author.Id.Value.ToString(),
                    Username = book.Author.Username,
                    Role = book.Author.Role.Name,
                    AvatarName = book.Author.Avatar.ImageName
                }
                select new BookResult
                {
                    Id = book.Id.Value.ToString(),
                    Title = book.Title,
                    Description = book.Description,
                    Author = user,
                    Average = -1,
                    CoverName = book.Cover.ImageName,
                    Genres = book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
                        .ToList()
                }
            )
            .PaginationAsync(page, limit);
        result.Items = result.Items.Select(book =>
        {
            book.Average = AverageRating(new Guid(book.Id));
            return book;
        }).ToArray();
        return result;
    }

    public async Task<PaginatedArray<BookResult>?>? GetPaginatedBookshelfBooks(Guid bookshelfId, int limit, int page)
    {
        if (!await AnyById(bookshelfId)) return null;
        var bookshelf = await _dbContext.Bookshelfes
            .Include(bookshelf => bookshelf.BookshelfBooks)
            .SingleAsync(bookshelf => bookshelf.Id == BookshelfId.CreateBookshelfId(bookshelfId));
        var books = await 
            (
                from bookshelfBook in bookshelf.BookshelfBooks
                let book = bookshelfBook.Book
                let user = new UserResult
                {
                    Id = book.Author.Id.Value.ToString(),
                    Username = book.Author.Username,
                    Role = book.Author.Role.Name,
                    AvatarName = book.Author.Avatar.ImageName
                }
                select new BookResult
                {
                    Id = book.Id.Value.ToString(),
                    Title = book.Title,
                    Description = book.Description,
                    Author = user,
                    Average = AverageRating(book.Id.Value),
                    CoverName = book.Cover.ImageName,
                    Genres = book.Genres.Select(genre => new GenreResult { Id = genre.Id.Value, Name = genre.Name })
                        .ToList()
                }
            )
            .AsQueryable()
            .PaginationAsync(page, limit);
        return books;

    }

    public async Task<Book?> GetSingleById(Guid guid)
    {
        return await _dbContext.Books.SingleOrDefaultAsync(book => book.Id == BookId.CreateBookId(guid));
    }

    public async Task<bool> AnyById(Guid guid)
    {
        return await _dbContext.Books.AnyAsync(book => book.Id == BookId.CreateBookId(guid));
    }

    public double AverageRating(Guid bookId)
    {
        var reviews = _dbContext.Reviews
            .Include(review => review.Book)
            .Where(review => review.Book.Id == BookId.CreateBookId(bookId));
        if (reviews.Any())
        {
            return reviews.Average(review => review.Rating);
        }

        return 0;
    }
}