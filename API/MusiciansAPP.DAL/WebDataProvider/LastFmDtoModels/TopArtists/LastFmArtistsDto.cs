using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;

internal class LastFmArtistsDto
{
    public LastFmArtistsDto()
    {
        Artists = new List<LastFmArtistDto>();
    }

    [JsonProperty(PropertyName = "artist")]
    public IEnumerable<LastFmArtistDto> Artists { get; set; }
}