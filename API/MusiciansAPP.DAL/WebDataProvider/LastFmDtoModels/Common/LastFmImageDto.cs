using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.Common;

internal class LastFmImageDto
{
    [JsonProperty(PropertyName = "#text")]
    public string Url { get; set; }

    [JsonProperty(PropertyName = "size")]
    public string Size { get; set; }
}