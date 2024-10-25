namespace elite_shop.Exceptions;

public class EmailAlreadyInUseException : Exception
{
    public EmailAlreadyInUseException() : base("Email already in use.") { }
}

