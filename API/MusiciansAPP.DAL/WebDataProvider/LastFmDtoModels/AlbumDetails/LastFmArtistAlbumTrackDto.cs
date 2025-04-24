using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumTrackDto
{
    [JsonProperty(PropertyName = "track")]
    public LastFmAlbumTrackDto Track { get; set; }
}