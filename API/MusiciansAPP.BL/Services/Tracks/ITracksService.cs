using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.Services.Tracks;

public interface ITracksService
{
    Task<IEnumerable<TrackBL>> GetArtistTopTracksAsync(string name, int pageSize, int page);

    Task<int> GetTracksAmountAsync(string artistName);
}