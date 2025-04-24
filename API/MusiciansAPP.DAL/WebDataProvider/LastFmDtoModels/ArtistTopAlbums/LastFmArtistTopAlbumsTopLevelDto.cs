using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;

internal class LastFmArtistTopAlbumsTopLevelDto
{
    [JsonProperty(PropertyName = "topalbums")]
    public LastFmArtistTopAlbumsDto TopLevel { get; set; }
}