using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;

internal class LastFmArtistBiographyDto
{
    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }
}