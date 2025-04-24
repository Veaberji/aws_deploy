using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums;

namespace MusiciansAPP.API.Controllers;

[Route("api/albums")]
public class AlbumsController : AppController
{
    private const int DefaultSize = 10;
    private const int DefaultPage = 1;

    private readonly IAlbumsService _albumsService;
    private readonly IMapper _mapper;
    private readonly IPagingHelper _pagingHelper;

    public AlbumsController(
        IErrorHandler errorHandler,
        IAlbumsService albumsService,
        IMapper mapper,
        IPagingHelper pagingHelper)
        : base(errorHandler)
    {
        _albumsService = albumsService;
        _mapper = mapper;
        _pagingHelper = pagingHelper;
    }

    [HttpGet("byDate")]
    public async Task<ActionResult<IEnumerable<AlbumUI>>> GetAlbumsByDate(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int pageSize,
        [FromQuery] int page = 1)
    {
        var func = async () =>
        {
            int size = _pagingHelper.GetCorrectPageSize(pageSize);
            var albums = await _albumsService.GetAlbumsByDatesAsync(
                DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate), size, page);

            return _mapper.Map<IEnumerable<AlbumUI>>(albums);
        };

        return await GetDataAsync(func, nameof(GetAlbumsByDate));
    }

    [HttpGet("byDate/amount")]
    public async Task<ActionResult<int>> GetAlbumsByDateAmount(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var func = async () =>
        {
            return await _albumsService.GetAlbumsAmountAsync(
                DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
        };

        return await GetDataAsync(func, nameof(GetAlbumsByDateAmount));
    }

    [HttpGet("{name}/top-albums")]
    public async Task<ActionResult<IEnumerable<AlbumUI>>> GetArtistTopAlbums(
        string name)
    {
        var func = async () =>
        {
            var models = await _albumsService.GetArtistTopAlbumsAsync(name, DefaultSize, DefaultPage);
            return _mapper.Map<IEnumerable<AlbumUI>>(models);
        };

        return await GetDataAsync(func, nameof(GetArtistTopAlbums));
    }

    [HttpGet("{artistName}/album-details/{albumName}")]
    public async Task<ActionResult<AlbumUI>> GetAlbumDetails(
        string artistName, string albumName)
    {
        var func = async () =>
        {
            var models = await _albumsService.GetArtistAlbumDetailsAsync(artistName, albumName);
            return _mapper.Map<AlbumUI>(models);
        };

        return await GetDataAsync(func, nameof(GetAlbumDetails));
    }
}