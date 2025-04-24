using System.Collections.Generic;
using System.Threading.Tasks;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context, IDataLogger<IUnitOfWork> logger)
    {
        _context = context;
        Artists = new ArtistRepository(_context, logger);
        Albums = new AlbumRepository(_context, logger);
        Tracks = new TrackRepository(_context, logger);
    }

    public IArtistRepository Artists { get; }

    public IAlbumRepository Albums { get; }

    public ITrackRepository Tracks { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task SaveAlbumDetailsAsync(Album album, IEnumerable<Track> tracks)
    {
        var addedAlbum = await Albums.AddOrUpdateAlbumDetailsAsync(album);
        addedAlbum.Description = album.Description;
        await UpdateAlbumTracksAsync(addedAlbum, tracks);
    }

    private async Task UpdateAlbumTracksAsync(
        Album album, IEnumerable<Track> tracks)
    {
        await Tracks.AddOrUpdateAlbumTracksAsync(album, tracks);
        await CompleteAsync();
    }
}