using System.Net;

namespace CineAPI.Common;

public class CineException : Exception
{
    public HttpStatusCode? HttpStatusCode { get; set; }

    public CineException()
        : base()
    {
    }

    public CineException(string message)
        : base(message)
    {
    }

    public CineException(string message, HttpStatusCode httpStatusCode)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public CineException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
