using PostsApp.Application.Common.Results;
using PostsApp.Application.Genres;
using PostsApp.Application.Users.Results;
using PostsApp.Domain.Genre;

namespace PostsApp.Application.Books.Results;

public class BookResult : RatingStatistics
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string ReferentialName { get; set; }
    public required string Description { get; set; }
    public required string CoverName { get; set; }
    public required UserResult Author { get; set; }
    public required List<GenreResult> Genres { get; set; }
}