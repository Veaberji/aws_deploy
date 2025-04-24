using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;

internal class LastFmArtistDetailsDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "bio")]
    public LastFmArtistBiographyDto Biography { get; set; }
}