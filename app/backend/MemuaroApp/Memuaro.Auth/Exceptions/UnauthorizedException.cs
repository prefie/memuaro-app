namespace Memuaro.Auth.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException()
    {
        
    }

    public UnauthorizedException(string message) : base(message)
    {
        
    }
}