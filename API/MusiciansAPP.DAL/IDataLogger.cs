using System.Runtime.CompilerServices;

namespace MusiciansAPP.DAL;

public interface IDataLogger<T>
{
    void LogDataSource([CallerMemberName] string data = null);
}