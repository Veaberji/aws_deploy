using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopAlbums;

internal class LastFmArtistTopAlbumsDto
{
    public LastFmArtistTopAlbumsDto()
    {
        Albums = new List<LastFmArtistTopAlbumDto>();
    }

    [JsonProperty(PropertyName = "album")]
    public IEnumerable<LastFmArtistTopAlbumDto> Albums { get; set; }

    [JsonProperty(PropertyName = "@attr")]
    public LastFmArtistTopAlbumsMetaDataDto MetaData { get; set; }
}