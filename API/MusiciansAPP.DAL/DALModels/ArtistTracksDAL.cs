using System.Collections.Generic;

namespace MusiciansAPP.DAL.DALModels;

public class ArtistTracksDAL
{
    public ArtistTracksDAL()
    {
        Tracks = new List<TrackDAL>();
    }

    public string ArtistName { get; set; }

    public IEnumerable<TrackDAL> Tracks { get; set; }
}