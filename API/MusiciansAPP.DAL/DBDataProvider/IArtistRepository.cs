using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public interface IArtistRepository : IRepository<Artist>
{
    Task<IEnumerable<Artist>> GetTopArtistsAsync(int pageSize, int page, CancellationToken token = default);

    Task<Artist> GetArtistDetailsAsync(string artistName);

    Task<Artist> GetArtistWithSimilarAsync(string artistName, int pageSize, int page);

    Task AddOrUpdateAsync(Artist artist);

    Task AddOrUpdateRangeAsync(IEnumerable<Artist> artists);

    Task AddOrUpdateSimilarArtistsAsync(string artistName, IEnumerable<Artist> similarArtists);

    Task<IEnumerable<Artist>> GetMatchedArtistsAsync(string name, int amount, CancellationToken token);
}