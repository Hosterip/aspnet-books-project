using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
using PostsApp.Application.Images.Commands.CreateImage;
using PostsApp.Application.Images.Commands.DeleteImage;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Book;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

[Route("[controller]")]
public class BooksController : Controller
{
    private readonly ISender _sender;

    public BooksController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("")]
    [Authorize(Policy = Policies.Author)]
    public async Task<IActionResult> Create([FromBodyOrDefault]BookRequest request, CancellationToken cancellationToken)
    {
        var imageCommand = new CreateImageCommand
        {
            Image = request.Cover
        };
        var fileName = await _sender.Send(imageCommand, cancellationToken);
        
        var createBookCommand = new CreateBookCommand
        {
            UserId = new Guid(HttpContext.GetId()!), 
            Title = request.Title,
            Description = request.Description,
            ImageName = fileName,
            GenreIds = request.GenreIds
        };
        var book = await _sender.Send(createBookCommand, cancellationToken);

        return StatusCode(201, book);
    }

    [HttpPut("")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Update([FromBodyOrDefault]UpdateBookRequest request, CancellationToken cancellationToken)
    {
        string? fileName = null;
        if (request.Cover is not null)
        {
            var bookQuery = new GetSingleBookQuery
            {
                Id = request.Id
            };
            var book = await _sender.Send(bookQuery, cancellationToken);
            var deleteImageCommand = new DeleteImageCommand
            {
                ImageName = book.CoverName
            };
            await _sender.Send(deleteImageCommand, cancellationToken);
            var imageCommand = new CreateImageCommand
            {
                Image = request.Cover
            };
            fileName = await _sender.Send(imageCommand, cancellationToken);
        }
        
        var updateBookCommand = new UpdateBookCommand
        {
            Id = request.Id, 
            UserId = new Guid(HttpContext.GetId()!),
            Title = request.Title, 
            Description = request.Description,
            ImageName = fileName ?? null,
            GenreIds = request.GenreIds
        };
        var result = await _sender.Send(updateBookCommand, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Policies.Authorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = new Guid(HttpContext.GetId()!) };
        await _sender.Send(command, cancellationToken);
        return Ok("Book was deleted");
    }

    [HttpGet("many")]
    public async Task<IActionResult> GetMany(
        CancellationToken cancellationToken,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? userId,
        [FromQuery] Guid? genreId
        )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId, GenreId = genreId};
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("single/{id:guid}")]
    public async Task<IActionResult> GetSingle(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = id };
        var post = await _sender.Send(query, cancellationToken);
        return Ok(post);
    }
}