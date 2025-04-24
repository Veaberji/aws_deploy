using System.Collections.Generic;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;

internal class LastFmArtistTopAlbumDto
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "playcount")]
    public int PlayCount { get; set; }

    [JsonProperty(PropertyName = "image")]
    public IEnumerable<LastFmImageDto> Images { get; set; }
}