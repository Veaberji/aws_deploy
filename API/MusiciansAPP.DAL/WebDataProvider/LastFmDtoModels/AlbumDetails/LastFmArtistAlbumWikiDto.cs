using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumWikiDto
{
    [JsonProperty(PropertyName = "published")]
    public string Published { get; set; }

    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }
}