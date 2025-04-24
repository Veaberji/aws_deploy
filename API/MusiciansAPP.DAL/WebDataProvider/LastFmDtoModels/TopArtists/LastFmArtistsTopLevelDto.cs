using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;

internal class LastFmArtistsTopLevelDto
{
    [JsonProperty(PropertyName = "artists")]
    public LastFmArtistsDto TopLevel { get; set; }
}