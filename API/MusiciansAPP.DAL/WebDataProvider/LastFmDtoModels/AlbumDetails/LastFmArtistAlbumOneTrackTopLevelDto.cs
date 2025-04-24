using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;

internal class LastFmArtistAlbumOneTrackTopLevelDto
{
    [JsonProperty(PropertyName = "album")]
    public LastFmArtistAlbumOneTrackDto TopLevel { get; set; }
}