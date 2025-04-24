using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider.WikipediaDtoModels;

internal class ImageResponseTopLevel
{
    public ImageResponseTopLevel()
    {
        Query = new ImageQuery();
    }

    [JsonProperty(PropertyName = "batchcomplete")]
    public string BatchComplete { get; set; }

    [JsonProperty(PropertyName = "query")]
    public ImageQuery Query { get; set; }
}