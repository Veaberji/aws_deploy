using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class ArtistAlbumsDAL
{
    public ArtistAlbumsDAL()
    {
        Albums = new List<AlbumDAL>();
    }

    public string ArtistName { get; set; }

    public IEnumerable<AlbumDAL> Albums { get; set; }
}