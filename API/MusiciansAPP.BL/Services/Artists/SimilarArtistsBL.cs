using System.Collections.Generic;

namespace MusiciansAPP.BL.Services.Artists;

public class SimilarArtistsBL
{
    public SimilarArtistsBL()
    {
        Artists = new List<ArtistBL>();
    }

    public string ArtistName { get; set; }

    public IEnumerable<ArtistBL> Artists { get; set; }
}