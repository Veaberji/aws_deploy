using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists;

namespace MusiciansAPP.API.Controllers;

[Route("api/artists")]
public class ArtistsController : AppController
{
    private const int DefaultSize = 10;
    private const int DefaultPage = 1;

    private readonly IArtistsService _artistsService;
    private readonly IMapper _mapper;
    private readonly IPagingHelper _pagingHelper;

    public ArtistsController(
        IErrorHandler errorHandler,
        IArtistsService artistsService,
        IMapper mapper,
        IPagingHelper pagingHelper)
        : base(errorHandler)
    {
        _artistsService = artistsService;
        _mapper = mapper;
        _pagingHelper = pagingHelper;
    }

    [HttpGet("top")]
    public async Task<ActionResult<IEnumerable<ArtistUI>>> GetTopArtists(
        [FromQuery] int pageSize, [FromQuery] int page = 1)
    {
        var func = async () =>
        {
            int size = _pagingHelper.GetCorrectPageSize(pageSize);
            var artists = await _artistsService.GetTopArtistsAsync(size, page);
            return _mapper.Map<IEnumerable<ArtistUI>>(artists);
        };
        return await GetDataAsync(func, nameof(GetTopArtists));
    }

    [HttpGet("{name}/details")]
    public async Task<ActionResult<ArtistUI>> GetArtistDetails(string name)
    {
        var func = async () =>
        {
            var artist = await _artistsService.GetArtistDetailsAsync(name);
            return _mapper.Map<ArtistUI>(artist);
        };

        return await GetDataAsync(func, nameof(GetArtistDetails));
    }

    [HttpGet("{name}/similar")]
    public async Task<ActionResult<IEnumerable<ArtistUI>>> GetSimilarArtists(string name)
    {
        var func = async () =>
        {
            var models = await _artistsService.GetSimilarArtistsAsync(name, DefaultSize, DefaultPage);
            return _mapper.Map<IEnumerable<ArtistUI>>(models);
        };

        return await GetDataAsync(func, nameof(GetSimilarArtists));
    }
}