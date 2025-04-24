using System.Collections.Generic;
using System.Threading.Tasks;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public interface ITrackRepository : IRepository<Track>
{
    Task<IEnumerable<Track>> GetTopTracksForArtistAsync(string artistName, int pageSize, int page);

    Task AddOrUpdateArtistTracksAsync(Artist artist, IEnumerable<Track> tracks);

    Task AddOrUpdateAlbumTracksAsync(Album album, IEnumerable<Track> tracks);
}