using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class SimilarArtistsDAL
{
    public SimilarArtistsDAL()
    {
        Artists = new List<ArtistDAL>();
    }

    public string ArtistName { get; set; }

    public IEnumerable<ArtistDAL> Artists { get; set; }
}