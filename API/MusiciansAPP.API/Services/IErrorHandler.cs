using System;

namespace MusiciansAPP.API.Services;

public interface IErrorHandler
{
    void HandleError(Exception error, string method);
}