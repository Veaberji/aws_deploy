using System;
using System.Collections.Generic;

namespace MusiciansAPP.API.UIModels;

public class AlbumUI
{
    public AlbumUI()
    {
        Tracks = new List<TrackUI>();
    }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public int PlayCount { get; set; }

    public string ArtistName { get; set; }

    public DateTime DateCreated { get; set; }

    public string Description { get; set; }

    public IEnumerable<TrackUI> Tracks { get; set; }
}