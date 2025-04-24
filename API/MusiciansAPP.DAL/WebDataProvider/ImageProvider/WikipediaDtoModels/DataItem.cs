using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider.WikipediaDtoModels;

internal class DataItem
{
    [JsonProperty(PropertyName = "pageid")]
    public int PageId { get; set; }

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "original")]
    public WikipediaImage Image { get; set; }
}
