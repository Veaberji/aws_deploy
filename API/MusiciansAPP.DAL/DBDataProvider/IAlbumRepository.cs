using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> GetTopAlbumsForArtistAsync(string artistName, int pageSize, int page);

    Task<Album> GetAlbumDetailsAsync(string artistName, string albumName);

    Task AddOrUpdateArtistAlbumsAsync(Artist artist, IEnumerable<Album> albums);

    Task<Album> AddOrUpdateAlbumDetailsAsync(Album album);

    Task<IEnumerable<Album>> GetAlbumsByDateAsync(DateOnly startDate, DateOnly endDate, int pageSize, int page);

    Task<int> GetAlbumsByDateAmountAsync(DateOnly startDate, DateOnly endDate);
}