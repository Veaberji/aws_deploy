using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;

internal class LastFmArtistDetailsTopLevelDto
{
    [JsonProperty(PropertyName = "artist")]
    public LastFmArtistDetailsDto Artist { get; set; }
}