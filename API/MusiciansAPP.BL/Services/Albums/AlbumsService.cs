using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Albums;

public class AlbumsService : BaseService, IAlbumsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AlbumsService(
        IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AlbumBL>> GetArtistTopAlbumsAsync(
        string name, int pageSize, int page)
    {
        var getData = async () =>
        {
            var albumsFromDb = await _unitOfWork.Albums
                .GetTopAlbumsForArtistAsync(name, pageSize, page);
            if (Entity.IsFullData(albumsFromDb, pageSize))
            {
                return _mapper.Map<IEnumerable<AlbumBL>>(albumsFromDb);
            }

            var artistAlbums = await GetArtistTopAlbumsBlAsync(name, pageSize, page);
            await SaveArtistTopAlbumsAsync(artistAlbums);

            return artistAlbums.Albums;
        };

        var onDbException = async () =>
        {
            return (await GetArtistTopAlbumsBlAsync(name, pageSize, page)).Albums;
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<AlbumBL> GetArtistAlbumDetailsAsync(
        string artistName, string albumName)
    {
        var getData = async () =>
        {
            var albumFromDb = await _unitOfWork.Albums.GetAlbumDetailsAsync(artistName, albumName);
            if (albumFromDb?.IsAlbumDetailsUpToDate() ?? false)
            {
                return _mapper.Map<AlbumBL>(albumFromDb);
            }

            var albumDetails = await GetArtistAlbumDetailsBlAsync(artistName, albumName);
            await SaveArtistAlbumDetailsAsync(albumDetails);

            return albumDetails;
        };

        var onDbException = async () =>
        {
            return await GetArtistAlbumDetailsBlAsync(artistName, albumName);
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<IEnumerable<AlbumBL>> GetAlbumsByDatesAsync(
        DateOnly startDate,
        DateOnly endDate,
        int pageSize,
        int page)
    {
        var getData = async () =>
        {
            var albumsFromDb = await _unitOfWork.Albums.GetAlbumsByDateAsync(startDate, endDate, pageSize, page);

            return _mapper.Map<IEnumerable<AlbumBL>>(albumsFromDb);
        };

        return await GetDataAsync(getData);
    }

    public async Task<int> GetAlbumsAmountAsync(DateOnly startDate, DateOnly endDate)
    {
        var getData = async () =>
        {
            return await _unitOfWork.Albums.GetAlbumsByDateAmountAsync(startDate, endDate);
        };

        return await GetDataAsync(getData);
    }

    private async Task<ArtistAlbumsBL> GetArtistTopAlbumsBlAsync(
        string name, int pageSize, int page)
    {
        var artistAlbums = await _webDataProvider.GetArtistTopAlbumsAsync(name, pageSize, page);
        return _mapper.Map<ArtistAlbumsBL>(artistAlbums);
    }

    private async Task SaveArtistTopAlbumsAsync(ArtistAlbumsBL model)
    {
        var albums = _mapper.Map<IEnumerable<Album>>(model.Albums);
        var artist =
            await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);
        await _unitOfWork.Albums.AddOrUpdateArtistAlbumsAsync(artist, albums);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<AlbumBL> GetArtistAlbumDetailsBlAsync(
        string artistName, string albumName)
    {
        var albumDetails = await _webDataProvider
            .GetArtistAlbumDetailsAsync(artistName, albumName);
        return _mapper.Map<AlbumBL>(albumDetails);
    }

    private async Task SaveArtistAlbumDetailsAsync(AlbumBL model)
    {
        var album = _mapper.Map<Album>(
            model,
            opts: opt =>
                opt.AfterMap((_, dest) => dest.Tracks = new List<Track>()));
        await AddArtistToAlbumAsync(model.ArtistName, album);
        var tracks = _mapper.Map<IEnumerable<Track>>(model.Tracks);
        await _unitOfWork.SaveAlbumDetailsAsync(album, tracks);
    }

    private async Task AddArtistToAlbumAsync(string artistName, Album newAlbum)
    {
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(artistName);
        newAlbum.Artist = artist;
    }
}