namespace elite_shop.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message) { }
}
