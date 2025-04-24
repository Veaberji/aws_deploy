using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;

internal class LastFmSimilarArtistsTopLevelDto
{
    [JsonProperty(PropertyName = "similarartists")]
    public LastFmSimilarArtistsDto TopLevel { get; set; }
}