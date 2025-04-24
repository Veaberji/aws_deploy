using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;

internal class LastFmArtistTopTracksTopLevelDto
{
    [JsonProperty(PropertyName = "toptracks")]
    public LastFmArtistTopTracksDto TopLevel { get; set; }
}