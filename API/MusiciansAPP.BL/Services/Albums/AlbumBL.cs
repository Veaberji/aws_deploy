using System;
using System.Collections.Generic;
using MusiciansAPP.BL.Services.Tracks;

namespace MusiciansAPP.BL.Services.Albums;

public class AlbumBL
{
    public AlbumBL()
    {
        Tracks = new List<TrackBL>();
    }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public int PlayCount { get; set; }

    public string ArtistName { get; set; }

    public DateOnly DateCreated { get; set; }

    public string Description { get; set; }

    public IEnumerable<TrackBL> Tracks { get; set; }
}