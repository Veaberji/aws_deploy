using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.Albums;

public class ArtistAlbumsBL
{
    public ArtistAlbumsBL()
    {
        Albums = new List<AlbumBL>();
    }

    public string ArtistName { get; set; }

    public IEnumerable<AlbumBL> Albums { get; set; }
}