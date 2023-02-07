using System.Runtime.Serialization;

namespace Flare.Exceptions;

public class FlareApiInitException :
    Exception
{
    public FlareApiInitException()
    {
    }

    protected FlareApiInitException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public FlareApiInitException(string? message) : base(message)
    {
    }

    public FlareApiInitException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}