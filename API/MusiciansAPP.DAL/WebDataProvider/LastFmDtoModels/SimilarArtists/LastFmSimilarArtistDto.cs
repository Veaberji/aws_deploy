using System.Collections.Generic;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;

internal class LastFmSimilarArtistDto
{
    public LastFmSimilarArtistDto()
    {
        Images = new List<LastFmImageDto>();
    }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "playcount")]
    public int Playcount { get; set; }

    [JsonProperty(PropertyName = "listeners")]
    public int Listeners { get; set; }

    [JsonProperty(PropertyName = "mbid")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "streamable")]
    public int Streamable { get; set; }

    [JsonProperty(PropertyName = "image")]
    public IEnumerable<LastFmImageDto> Images { get; set; }
}