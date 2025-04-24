using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider.WikipediaDtoModels;

internal class WikipediaImage
{
    [JsonProperty(PropertyName = "source")]
    public string ImageUrl { get; set; }

    [JsonProperty(PropertyName = "width")]
    public int Width { get; set; }

    [JsonProperty(PropertyName = "height")]
    public int Height { get; set; }
}
