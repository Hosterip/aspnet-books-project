namespace BooksApp.Contracts.Requests.Books;

public class GetBooksRequest
{
    public required int? Limit { get; set; }
    public required int? Page { get; set; }
    public required string? Q { get; set; }
    public required Guid? GenreId { get; set; }
}