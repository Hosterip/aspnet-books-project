namespace PostsApp.Domain.Exceptions;

public class PostException : Exception
{
    public PostException(string message) : base(message) {}
}