using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;

internal class LastFmArtistTopTrackDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "playcount")]
    public int? PlayCount { get; set; }
}