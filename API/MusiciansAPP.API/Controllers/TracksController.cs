using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Tracks;

namespace MusiciansAPP.API.Controllers;

[Route("api/tracks")]
public class TracksController : AppController
{
    private readonly ITracksService _tracksService;
    private readonly IMapper _mapper;
    private readonly IPagingHelper _pagingHelper;

    public TracksController(
        IErrorHandler errorHandler,
        ITracksService tracksService,
        IMapper mapper,
        IPagingHelper pagingHelper)
        : base(errorHandler)
    {
        _mapper = mapper;
        _tracksService = tracksService;
        _pagingHelper = pagingHelper;
    }

    [HttpGet("{name}/top-tracks")]
    public async Task<ActionResult<IEnumerable<TrackUI>>> GetArtistTopTracks(
        string name, [FromQuery] int pageSize, [FromQuery] int page = 1)
    {
        var func = async () =>
        {
            int size = _pagingHelper.GetCorrectPageSize(pageSize);
            var models = await _tracksService.GetArtistTopTracksAsync(name, size, page);
            return _mapper.Map<IEnumerable<TrackUI>>(models);
        };

        return await GetDataAsync(func, nameof(GetArtistTopTracks));
    }

    [HttpGet("{name}/top-tracks/amount")]
    public async Task<ActionResult<int>> GetArtistTopTracksAmount(string name)
    {
        var func = async () =>
        {
            return await _tracksService.GetTracksAmountAsync(name);
        };

        return await GetDataAsync(func, nameof(GetArtistTopTracksAmount));
    }
}