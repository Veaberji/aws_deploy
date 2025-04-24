using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;

internal class LastFmSimilarArtistsDto
{
    public LastFmSimilarArtistsDto()
    {
        Artists = new List<LastFmSimilarArtistDto>();
    }

    [JsonProperty(PropertyName = "artist")]
    public IEnumerable<LastFmSimilarArtistDto> Artists { get; set; }

    [JsonProperty(PropertyName = "@attr")]
    public LastFmSimilarArtistMetaDataDto MetaData { get; set; }
}