using System.Collections.Generic;
using System.Threading.Tasks;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public interface IUnitOfWork
{
    IArtistRepository Artists { get; }

    IAlbumRepository Albums { get; }

    ITrackRepository Tracks { get; }

    Task<int> CompleteAsync();

    Task SaveAlbumDetailsAsync(Album album, IEnumerable<Track> tracks);
}