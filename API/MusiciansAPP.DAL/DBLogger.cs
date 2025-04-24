using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using MusiciansAPP.DAL.DBDataProvider;

namespace MusiciansAPP.DAL;
public class DBLogger : IDataLogger<IUnitOfWork>
{
    private readonly ILogger _logger;

    public DBLogger(ILogger<DBLogger> logger)
    {
        _logger = logger;
    }

    public void LogDataSource([CallerMemberName] string method = null)
    {
        _logger.LogInformation($"'{method}' loaded data from the DB");
    }
}
