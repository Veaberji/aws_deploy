using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;

internal class LastFmArtistsMetaDataDto
{
    [JsonProperty(PropertyName = "page")]
    public int Page { get; set; }

    [JsonProperty(PropertyName = "perPage")]
    public int PageSize { get; set; }

    [JsonProperty(PropertyName = "totalPages")]
    public int TotalPages { get; set; }

    [JsonProperty(PropertyName = "total")]
    public int Total { get; set; }
}