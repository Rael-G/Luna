namespace Luna;

public class ResourceException: LunaException
{

    public ResourceException(string message) : base(message)
    {

    }
    
    public ResourceException(string message, Exception innerException) : base(message, innerException)
    {
        
    }

}
