namespace Luna;

public class LunaException : Exception
{

    public LunaException(string message) : base(message)
    {
        
    }
    
    public LunaException(string message, Exception innerException) : base(message, innerException)
    {

    }

}
