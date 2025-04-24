using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusiciansAPP.DAL.DBDataProvider;
using MusiciansAPP.DAL.WebDataProvider;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Tracks;

public class TracksService : BaseService, ITracksService
{
    private readonly IWebDataProvider _webDataProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TracksService(
        IWebDataProvider webDataProvider,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _webDataProvider = webDataProvider;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(
        string name, int pageSize, int page)
    {
        var getData = async () =>
        {
            var tracksFromDb = await _unitOfWork.Tracks
                .GetTopTracksForArtistAsync(name, pageSize, page);
            if (Entity.IsFullData(tracksFromDb, pageSize))
            {
                return _mapper.Map<IEnumerable<TrackBL>>(tracksFromDb);
            }

            var artistTracks = await GetArtistTopTracksBlAsync(name, pageSize, page);
            await SaveArtistTopTracksAsync(artistTracks);

            return artistTracks.Tracks;
        };

        var onDbException = async () =>
        {
            return (await GetArtistTopTracksBlAsync(name, pageSize, page)).Tracks;
        };

        return await GetDataAsync(getData, onDbException);
    }

    public async Task<int> GetTracksAmountAsync(string artistName)
    {
        var getData = async () =>
        {
            return await _webDataProvider.GetTracksAmountAsync(artistName);
        };

        return await GetDataAsync(getData);
    }

    private async Task<ArtistTracksBL> GetArtistTopTracksBlAsync(string name, int pageSize, int page)
    {
        var artistTracks = await _webDataProvider.GetArtistTopTracksAsync(name, pageSize, page);
        return _mapper.Map<ArtistTracksBL>(artistTracks);
    }

    private async Task SaveArtistTopTracksAsync(ArtistTracksBL model)
    {
        var tracks = _mapper.Map<IEnumerable<Track>>(model.Tracks);
        var artist = await _unitOfWork.Artists.GetArtistDetailsAsync(model.ArtistName);

        await _unitOfWork.Tracks.AddOrUpdateArtistTracksAsync(artist, tracks);
        await _unitOfWork.CompleteAsync();
    }
}