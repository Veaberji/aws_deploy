using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Artists;

public class ArtistsService : BaseService, IArtistsService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistsService(
        IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ArtistBL>> GetTopArtistsAsync(int pageSize, int page)
    {
        var getData = async () =>
        {
            var artistsFromDb = await _unitOfWork.Artists
                .GetTopArtistsAsync(pageSize, page);
            if (Entity.IsFullData(artistsFromDb, pageSize))
            {
                return _mapper.Map<IEnumerable<ArtistBL>>(artistsFromDb);
            }

            var topArtists = await GetTopArtistsBlAsync(pageSize, page);
            await SaveTopArtistsAsync(topArtists);

            return topArtists;
        };

        var onDbException = async () =>
        {
            return await GetTopArtistsBlAsync(pageSize, page);
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<ArtistBL> GetArtistDetailsAsync(string name)
    {
        var getData = async () =>
        {
            var artistFromDb = await _unitOfWork.Artists.GetArtistDetailsAsync(name);
            if (artistFromDb.IsArtistDetailsUpToDate())
            {
                return _mapper.Map<ArtistBL>(artistFromDb);
            }

            var artistDetails = await GetArtistDetailsBlAsync(name);
            await SaveArtistDetailsAsync(artistDetails);

            return artistDetails;
        };

        var onDbException = async () =>
        {
            return await GetArtistDetailsBlAsync(name);
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<IEnumerable<ArtistBL>> GetSimilarArtistsAsync(
        string name, int pageSize, int page)
    {
        var getData = async () =>
        {
            var artistsFromDb = await _unitOfWork.Artists
                .GetArtistWithSimilarAsync(name, pageSize, page);
            if (Entity.IsFullData(artistsFromDb.SimilarArtists, pageSize))
            {
                return _mapper.Map<IEnumerable<ArtistBL>>(artistsFromDb.SimilarArtists);
            }

            var similarArtists = await GetSimilarArtistsBlAsync(name, pageSize, page);
            await SaveSimilarArtistsAsync(similarArtists);

            return similarArtists.Artists;
        };

        var onDbException = async () =>
        {
            return (await GetSimilarArtistsBlAsync(name, pageSize, page)).Artists;
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<IEnumerable<ArtistBL>> GetMatchedArtistsAsync(string name, int amount, CancellationToken token)
    {
        var getData = async () =>
        {
            var artists = string.IsNullOrWhiteSpace(name)
                ? await _unitOfWork.Artists.GetTopArtistsAsync(pageSize: amount, page: 1, token)
                : await _unitOfWork.Artists.GetMatchedArtistsAsync(name, amount, token);
            return _mapper.Map<IEnumerable<ArtistBL>>(artists);
        };

        return await GetDataAsync(getData);
    }

    private async Task<IEnumerable<ArtistBL>> GetTopArtistsBlAsync(int pageSize, int page)
    {
        var topArtists = await _webDataProvider.GetTopArtistsAsync(pageSize, page);
        return _mapper.Map<IEnumerable<ArtistBL>>(topArtists);
    }

    private async Task SaveTopArtistsAsync(IEnumerable<ArtistBL> models)
    {
        var artists = _mapper.Map<IEnumerable<Artist>>(models);
        await _unitOfWork.Artists.AddOrUpdateRangeAsync(artists);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<ArtistBL> GetArtistDetailsBlAsync(string name)
    {
        var artistDetails = await _webDataProvider.GetArtistDetailsAsync(name);
        return _mapper.Map<ArtistBL>(artistDetails);
    }

    private async Task SaveArtistDetailsAsync(ArtistBL model)
    {
        var artist = _mapper.Map<Artist>(model);
        await _unitOfWork.Artists.AddOrUpdateAsync(artist);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<SimilarArtistsBL> GetSimilarArtistsBlAsync(string name, int pageSize, int page)
    {
        var similarArtists = await _webDataProvider.GetSimilarArtistsAsync(name, pageSize, page);
        return _mapper.Map<SimilarArtistsBL>(similarArtists);
    }

    private async Task SaveSimilarArtistsAsync(SimilarArtistsBL model)
    {
        var similarArtists =
            _mapper.Map<IEnumerable<Artist>>(model.Artists);
        await _unitOfWork.Artists
            .AddOrUpdateSimilarArtistsAsync(model.ArtistName, similarArtists);
        await _unitOfWork.CompleteAsync();
    }
}