using System;
using Microsoft.Extensions.Logging;

namespace MusiciansAPP.API.Services;

public class ErrorHandler : IErrorHandler
{
    private readonly ILogger _logger;

    public ErrorHandler(ILogger<ErrorHandler> logger)
    {
        _logger = logger;
    }

    public void HandleError(Exception error, string method)
    {
        LogError(error, method);
    }

    private void LogError(Exception error, string method)
    {
        _logger.LogError($"Exception in {method}, {error.Message}");
    }
}