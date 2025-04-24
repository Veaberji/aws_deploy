using System;

namespace MusiciansAPP.BL.Exceptions;

public class DataServiceUnavailableException : Exception
{
    public DataServiceUnavailableException()
    {
    }

    public DataServiceUnavailableException(string message)
        : base(message)
    {
    }

    public DataServiceUnavailableException(string message, Exception innerException)
    : base(message, innerException)
    {
    }
}
