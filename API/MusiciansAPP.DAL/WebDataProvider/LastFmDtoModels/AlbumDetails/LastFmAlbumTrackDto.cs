using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmAlbumTrackDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "duration")]
    public int? DurationInSeconds { get; set; }
}