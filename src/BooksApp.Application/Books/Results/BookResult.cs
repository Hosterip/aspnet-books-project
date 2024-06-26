using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Results;

public record BookResult
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required double Average { get; set; }
    public required string CoverName { get; set; }
    public required UserResult Author { get; set; }
}