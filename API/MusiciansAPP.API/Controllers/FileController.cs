using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists;
using MusiciansAPP.BL.Services.CSV;

namespace MusiciansAPP.API.Controllers;

[Route("api/file")]
public class FileController : AppController
{
    private readonly ICSVService<ArtistBL> _csvService;
    private readonly IArtistsService _artistsService;

    public FileController(
        IErrorHandler errorHandler,
        ICSVService<ArtistBL> csvService,
        IArtistsService artistsService)
    : base(errorHandler)
    {
        _csvService = csvService;
        _artistsService = artistsService;
    }

    [HttpPost("artists-report")]
    public async Task<ActionResult> GetArtistsReport(
        [FromBody] ArtistsReportRequest request, CancellationToken token)
    {
        var func = async () =>
        {
            var artists = await _artistsService.GetMatchedArtistsAsync(request.Name, request.Amount, token);
            byte[] file = _csvService.GetReport(artists);
            const string contentType = "text/csv";
            string fileName = _csvService.GetReportFileName(request.Name, artists.Count());

            return File(file, contentType, fileName);
        };

        return await GetFileAsync(func, nameof(GetArtistsReport));
    }
}
