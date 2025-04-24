using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    public ArtistRepository(DbContext context, IDataLogger<IUnitOfWork> logger)
        : base(context, logger)
    {
    }

    private DbSet<Artist> Artists => (Context as AppDbContext)?.Artists;

    public async Task<IEnumerable<Artist>> GetTopArtistsAsync(int pageSize, int page, CancellationToken token = default)
    {
        var artists = await Artists
            .OrderBy(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        Logger.LogDataSource();

        return artists;
    }

    public async Task<Artist> GetArtistDetailsAsync(string artistName)
    {
        var artist = await Artists
            .Where(a => a.Name == artistName)
            .FirstOrDefaultAsync() ?? new Artist
            {
                Name = artistName,
            };

        Logger.LogDataSource();

        return artist;
    }

    public async Task<Artist> GetArtistWithSimilarAsync(
        string artistName, int pageSize, int page)
    {
        var artist = await Artists.Where(a => a.Name == artistName)
            .Include(a => a.SimilarArtists
            .OrderBy(a => a.Id)
            .Take(pageSize))
            .FirstOrDefaultAsync() ?? new Artist();

        Logger.LogDataSource();

        return artist;
    }

    public async Task AddOrUpdateAsync(Artist artist)
    {
        var artistFromDb = (await FindAsync(a => a.Name == artist.Name)).FirstOrDefault();
        if (artistFromDb is null)
        {
            await AddAsync(artist);
        }
        else if (!artistFromDb.IsArtistDetailsUpToDate())
        {
            UpdateArtistDetails(artist, artistFromDb);
        }
    }

    public async Task AddOrUpdateRangeAsync(IEnumerable<Artist> artists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(artists);
        var artistsFromDbNames = artistsFromDb.Select(a => a.Name).ToList();
        var newArtists = artists
            .Where(artist => IsNewItem(artistsFromDbNames, artist.Name))
            .ToList();
        await AddRangeAsync(newArtists);

        UpdateArtistImageUrl(artists, artistsFromDb);
    }

    public async Task AddOrUpdateSimilarArtistsAsync(
        string artistName, IEnumerable<Artist> similarArtists)
    {
        var artistInDB = await GetArtistWithSimilarAsync(
            artistName, pageSize: int.MaxValue, page: 1);
        if (artistInDB is null)
        {
            var newArtist = new Artist
            { Name = artistName };

            await AddSimilarToArtistAsync(newArtist, similarArtists);
            await AddAsync(newArtist);
        }
        else
        {
            await AddSimilarToArtistAsync(artistInDB, similarArtists);
        }
    }

    public async Task<IEnumerable<Artist>> GetMatchedArtistsAsync(
        string name, int amount, CancellationToken token)
    {
        var primaryMatchedArtists = await Artists
            .Where(a => a.Name.ToLower().StartsWith(name))
            .OrderBy(a => a.Id)
            .Take(amount)
            .ToListAsync(token);

        var secondaryMatchedArtists = await Artists
            .Where(a => a.Name.ToLower().Contains(name)
                && !primaryMatchedArtists.Select(pa => pa.Id).Contains(a.Id))
            .OrderBy(a => a.Id)
            .Take(amount - primaryMatchedArtists.Count())
            .ToListAsync(token);

        Logger.LogDataSource();

        return primaryMatchedArtists.Concat(secondaryMatchedArtists);
    }

    private static void UpdateArtistImageUrl(
        IEnumerable<Artist> artists, IEnumerable<Artist> artistsFromDb)
    {
        foreach (var artistFromDb in artistsFromDb.Where(a => !a.IsArtistHasImageUrl()))
        {
            var artist = artists.First(a => a.Name == artistFromDb.Name);
            artistFromDb.ImageUrl = artist.ImageUrl;
        }
    }

    private static void UpdateArtistDetails(Artist artist, Artist artistFromDb)
    {
        artistFromDb.Biography = artist.Biography;
        artistFromDb.ImageUrl = artist.ImageUrl;
    }

    private static IEnumerable<Artist> GetNewSimilarArtists(
        Artist artist, IEnumerable<Artist> artists)
    {
        var existingSimilarArtistsIds = artist.SimilarArtists.Select(a => a.Id).ToList();
        var newSimilarArtists = artists
            .Where(artist => IsNewSimilarArtist(existingSimilarArtistsIds, artist))
            .ToList();

        return newSimilarArtists;
    }

    private static bool IsNewSimilarArtist(
        IEnumerable<Guid> existingSimilarArtistsIds, Artist artist)
    {
        return !existingSimilarArtistsIds.Contains(artist.Id);
    }

    private async Task<IEnumerable<Artist>> GetArtistsFromDbAsync(
        IEnumerable<Artist> artists)
    {
        var artistsNames = artists.Select(a => a.Name).ToList();
        return await FindAsync(a => artistsNames.Contains(a.Name));
    }

    private async Task AddSimilarToArtistAsync(
        Artist artist, IEnumerable<Artist> similarArtists)
    {
        var artists = await GetSimilarArtistsAsync(similarArtists);
        var newSimilarArtists = GetNewSimilarArtists(artist, artists);
        artist.SimilarArtists.AddRange(newSimilarArtists);
    }

    private async Task<List<Artist>> GetSimilarArtistsAsync(
        IEnumerable<Artist> similarArtists)
    {
        var artistsFromDb = await GetArtistsFromDbAsync(similarArtists);
        var newArtists = CreateNotExistingArtists(similarArtists, artistsFromDb);
        var artists = artistsFromDb.Concat(newArtists).ToList();

        return artists;
    }

    private IEnumerable<Artist> CreateNotExistingArtists(
        IEnumerable<Artist> artists, IEnumerable<Artist> artistsFromDb)
    {
        var artistsFromDbNames = artistsFromDb.Select(a => a.Name).ToList();
        return artists
            .Where(artist => IsNewItem(artistsFromDbNames, artist.Name))
            .ToList();
    }
}