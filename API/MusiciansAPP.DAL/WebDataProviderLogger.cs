using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using MusiciansAPP.DAL.WebDataProvider;

namespace MusiciansAPP.DAL;

public class WebDataProviderLogger : IDataLogger<IWebDataProvider>
{
    private readonly ILogger _logger;

    public WebDataProviderLogger(ILogger<DBLogger> logger)
    {
        _logger = logger;
    }

    public void LogDataSource([CallerMemberName] string method = null)
    {
        _logger.LogInformation($"'{method}' loaded data from the IWebDataProvider");
    }
}