using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;

internal class LastFmArtistTopTracksDto
{
    public LastFmArtistTopTracksDto()
    {
        Tracks = new List<LastFmArtistTopTrackDto>();
    }

    [JsonProperty(PropertyName = "track")]
    public IEnumerable<LastFmArtistTopTrackDto> Tracks { get; set; }

    [JsonProperty(PropertyName = "@attr")]
    public LastFmArtistTopTracksMetaDataDto MetaData { get; set; }
}