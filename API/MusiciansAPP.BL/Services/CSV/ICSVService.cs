using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.CSV;

public interface ICSVService<T>
{
    byte[] GetReport(IEnumerable<T> lines);

    string GetReportFileName(string name, int amount);
}
