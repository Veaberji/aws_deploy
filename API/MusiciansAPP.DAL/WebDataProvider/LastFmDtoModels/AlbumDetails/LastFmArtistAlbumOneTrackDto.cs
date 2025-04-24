using System.Collections.Generic;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumOneTrackDto
{
    public LastFmArtistAlbumOneTrackDto()
    {
        Images = new List<LastFmImageDto>();
        Wiki = new LastFmArtistAlbumWikiDto();
    }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "artist")]
    public string ArtistName { get; set; }

    [JsonProperty(PropertyName = "playcount")]
    public int PlayCount { get; set; }

    [JsonProperty(PropertyName = "image")]
    public IEnumerable<LastFmImageDto> Images { get; set; }

    [JsonProperty(PropertyName = "tracks")]
    public LastFmArtistAlbumTrackDto Track { get; set; }

    [JsonProperty(PropertyName = "wiki")]
    public LastFmArtistAlbumWikiDto Wiki { get; set; }
}