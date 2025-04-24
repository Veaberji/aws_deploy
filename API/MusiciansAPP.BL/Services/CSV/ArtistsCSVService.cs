using System.Collections.Generic;
using System.Linq;
using MusiciansAPP.BL.Services.Artists;

namespace MusiciansAPP.BL.Services.CSV;

public class ArtistsCSVService : CSVService, ICSVService<ArtistBL>
{
    private const string Separator = ",";

    public byte[] GetReport(IEnumerable<ArtistBL> artists)
    {
        const string columns = $"{nameof(ArtistBL.Name)}{Separator}{nameof(ArtistBL.ImageUrl)}{Separator}{nameof(ArtistBL.Biography)}";
        var lines = artists
            .Select(a => $"\"{a.Name}\" {Separator} {a.ImageUrl} {Separator} {a.Biography?.Replace("\r\n", " ").Replace("\n", " ").Replace("\t", " ")}")
            .ToList();
        lines.Insert(0, columns);
        return GetCSVFromLines(lines);
    }

    public string GetReportFileName(string name, int amount)
    {
        string artistName = string.IsNullOrWhiteSpace(name) ? "artists" : name;
        return GetCSVFileName(artistName, amount);
    }
}