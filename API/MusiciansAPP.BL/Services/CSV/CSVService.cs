using System;
using System.Collections.Generic;
using System.Text;

namespace MusiciansAPP.BL.Services.CSV;

public abstract class CSVService
{
    protected static byte[] GetCSVFromLines(IEnumerable<string> lines)
    {
        string csv = string.Join(Environment.NewLine, lines);
        return Encoding.UTF8.GetBytes(csv);
    }

    protected static string GetCSVFileName(string name, int amount)
    {
        string definition = amount == 1 ? "line" : "lines";

        return $"{name}_{amount}-{definition}_report.csv";
    }
}
