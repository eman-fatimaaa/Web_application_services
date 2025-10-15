namespace WebApplication1.Exceptions;

public class InvalidForeignKeyException : Exception
{
    public InvalidForeignKeyException() { }
    public InvalidForeignKeyException(string? message) : base(message) { }
    public InvalidForeignKeyException(string? message, Exception? innerException) : base(message, innerException) { }
}