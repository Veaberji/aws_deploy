using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.Exceptions;
using MusiciansAPP.DAL.WebDataProvider.ImageProvider;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider;

public class LastFmDataProvider : IWebDataProvider
{
    private const string BaseUrl = "http://ws.audioscrobbler.com/2.0/";
    private readonly string _apiKey;
    private readonly IMapper _mapper;
    private readonly IHttpClient _httpClient;
    private readonly IImageProvider _imageProvider;

    private IDataLogger<IWebDataProvider> _logger;

    public LastFmDataProvider(
        string apiKey,
        IMapper mapper,
        IHttpClient httpClient,
        IImageProvider imageProvider,
        IDataLogger<IWebDataProvider> logger)
    {
        _apiKey = apiKey;
        _mapper = mapper;
        _httpClient = httpClient;
        _imageProvider = imageProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<ArtistDAL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var response = await GetTopArtistsResponseAsync(pageSize, page);
        if (!response.IsSuccessStatusCode)
        {
            return new List<ArtistDAL>();
        }

        var content = await GetResponseContentAsync(response);
        var result = JsonConvert.DeserializeObject<LastFmArtistsTopLevelDto>(content);

        // this line was added because last.fm returns the number of artists equal page * pageSize for some pages.
        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        var artists = _mapper.Map<IEnumerable<ArtistDAL>>(result.TopLevel.Artists);
        await SetArtistsImageUrlAsync(artists);

        _logger.LogDataSource();

        return artists;
    }

    public async Task<ArtistDAL> GetArtistDetailsAsync(string name)
    {
        var content = await GetArtistDetailsContentAsync(name);
        var result = JsonConvert.DeserializeObject<LastFmArtistDetailsTopLevelDto>(content);
        var artist = _mapper.Map<ArtistDAL>(result.Artist);
        await SetArtistImageUrlAsync(artist);

        _logger.LogDataSource();

        return artist;
    }

    public async Task<ArtistTracksDAL> GetArtistTopTracksAsync(string name, int pageSize, int page)
    {
        var content = await GetArtistTopTracksContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);

        result.TopLevel.Tracks = result.TopLevel.Tracks.TakeLast(pageSize);

        var tracks = _mapper.Map<ArtistTracksDAL>(result.TopLevel);

        _logger.LogDataSource();

        return tracks;
    }

    public async Task<ArtistAlbumsDAL> GetArtistTopAlbumsAsync(string name, int pageSize, int page)
    {
        var content = await GetArtistTopAlbumsContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmArtistTopAlbumsTopLevelDto>(content);

        result.TopLevel.Albums = result.TopLevel.Albums.TakeLast(pageSize);

        var model = _mapper.Map<ArtistAlbumsDAL>(result.TopLevel);

        await SetDateCreatedAsync(model);

        _logger.LogDataSource();

        return model;
    }

    public async Task<SimilarArtistsDAL> GetSimilarArtistsAsync(string name, int pageSize, int page)
    {
        var content = await GetSimilarArtistsContentAsync(name, pageSize, page);
        var result = JsonConvert.DeserializeObject<LastFmSimilarArtistsTopLevelDto>(content);

        result.TopLevel.Artists = result.TopLevel.Artists.TakeLast(pageSize);

        var model = _mapper.Map<SimilarArtistsDAL>(result.TopLevel);
        await SetArtistsImageUrlAsync(model.Artists);

        _logger.LogDataSource();

        return model;
    }

    public async Task<AlbumDAL> GetArtistAlbumDetailsAsync(string artistName, string albumName)
    {
        var response = await GetArtistAlbumDetailsResponseAsync(artistName, albumName);
        if (!response.IsSuccessStatusCode)
        {
            ThrowNotFoundError($"Album {albumName} by artist {artistName} not found");
        }

        var content = await GetResponseContentAsync(response);

        try
        {
            return GetDeserializedRegularAlbum(content);
        }
        catch (JsonSerializationException)
        {
            return GetDeserializedOneTrackAlbum(content);
        }
    }

    public async Task<int> GetTracksAmountAsync(string artistName)
    {
        var content = await GetArtistTopTracksContentAsync(artistName, pageSize: 1, page: 1);
        var result = JsonConvert.DeserializeObject<LastFmArtistTopTracksTopLevelDto>(content);

        _logger.LogDataSource();

        return result.TopLevel.MetaData.Total;
    }

    private static string EscapeName(string name)
    {
        const string ampersand = "%26";
        const string hash = "%23";
        const string plus = "%2B";
        return name.Replace("&", ampersand).Replace("#", hash).Replace("+", plus);
    }

    private static async Task<string> GetResponseContentAsync(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }

    private static void CheckResponseContent(string content, string name)
    {
        if (!IsArtistFound(content))
        {
            ThrowNotFoundError($"Artist {name} not found");
        }
    }

    private static bool IsArtistFound(string content)
    {
        const string lastFmErrorMessage = "The artist you supplied could not be found";
        return !content.Contains(lastFmErrorMessage);
    }

    private static void ThrowNotFoundError(string message)
    {
        throw new NotFoundException(message);
    }

    private async Task<HttpResponseMessage> GetTopArtistsResponseAsync(int pageSize, int page)
    {
        const string method = "chart.gettopartists";
        var url =
            $"{BaseUrl}?method={method}&page={page}&limit={pageSize}&api_key={_apiKey}&format=json";

        return await GetResponseAsync(url);
    }

    private async Task<string> GetArtistDetailsContentAsync(string name)
    {
        const string method = "artist.getinfo";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(name)}&api_key={_apiKey}&format=json";

        return await GetContentAsync(url, name);
    }

    private async Task<string> GetArtistTopTracksContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettoptracks";
        var url = GetUrlForSupplements(method, name, pageSize, page);

        return await GetContentAsync(url, name);
    }

    private async Task<string> GetArtistTopAlbumsContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.gettopalbums";
        var url = GetUrlForSupplements(method, name, pageSize, page);

        return await GetContentAsync(url, name);
    }

    private async Task SetDateCreatedAsync(ArtistAlbumsDAL model)
    {
        foreach (var album in model.Albums)
        {
            try
            {
                var albumDetails = await GetArtistAlbumDetailsAsync(model.ArtistName, album.Name);
                album.DateCreated = albumDetails.DateCreated;
            }
            catch
            {
                album.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            }
        }
    }

    private async Task<string> GetSimilarArtistsContentAsync(string name, int pageSize, int page)
    {
        const string method = "artist.getsimilar";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(name)}&limit={pageSize * page}&api_key={_apiKey}&format=json";

        return await GetContentAsync(url, name);
    }

    private async Task<HttpResponseMessage> GetArtistAlbumDetailsResponseAsync(
        string artistName, string albumName)
    {
        const string method = "album.getinfo";
        var url =
            $"{BaseUrl}?method={method}&artist={EscapeName(artistName)}&album={EscapeName(albumName)}&api_key={_apiKey}&format=json";

        return await GetResponseAsync(url);
    }

    private async Task<string> GetContentAsync(string url, string artistName)
    {
        var response = await GetResponseAsync(url);
        var content = await GetResponseContentAsync(response);
        CheckResponseContent(content, artistName);

        return content;
    }

    private string GetUrlForSupplements(string method, string name, int pageSize, int page)
    {
        return $"{BaseUrl}?method={method}&artist={EscapeName(name)}&limit={pageSize}&page={page}&api_key={_apiKey}&format=json";
    }

    private async Task<HttpResponseMessage> GetResponseAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }

    private AlbumDAL GetDeserializedRegularAlbum(string content)
    {
        var result = JsonConvert
            .DeserializeObject<LastFmArtistAlbumTopLevelDto>(content);

        var album = _mapper.Map<AlbumDAL>(result.TopLevel);

        _logger.LogDataSource();

        return album;
    }

    private AlbumDAL GetDeserializedOneTrackAlbum(string content)
    {
        var result = JsonConvert
            .DeserializeObject<LastFmArtistAlbumOneTrackTopLevelDto>(content);
        var album = _mapper.Map<AlbumDAL>(result.TopLevel);

        _logger.LogDataSource();

        return album;
    }

    private async Task SetArtistImageUrlAsync(ArtistDAL artist)
    {
        var image = await _imageProvider.GetArtistImageUrlAsync(artist.Name);
        artist.ImageUrl = image;
    }

    private async Task SetArtistsImageUrlAsync(IEnumerable<ArtistDAL> artists)
    {
        foreach (var artist in artists)
        {
            await SetArtistImageUrlAsync(artist);
        }
    }
}