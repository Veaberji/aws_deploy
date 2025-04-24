using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;

internal class LastFmSimilarArtistMetaDataDto
{
    [JsonProperty(PropertyName = "artist")]
    public string ArtistName { get; set; }
}