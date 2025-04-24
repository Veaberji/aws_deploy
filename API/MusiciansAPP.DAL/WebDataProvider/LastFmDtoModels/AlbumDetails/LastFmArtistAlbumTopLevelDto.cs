using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumTopLevelDto
{
    [JsonProperty(PropertyName = "album")]
    public LastFmArtistAlbumDto TopLevel { get; set; }
}