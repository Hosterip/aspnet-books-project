namespace PostsApp.Common.Contracts.Responses.User;

public class UserResponse
{
    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string Role { get; init; }
    public required string? AvatarName { get; init; }
}