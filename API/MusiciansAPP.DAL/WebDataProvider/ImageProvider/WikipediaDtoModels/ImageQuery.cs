using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider.WikipediaDtoModels;

internal class ImageQuery
{
    public ImageQuery()
    {
        DataItems = new Dictionary<string, DataItem>();
    }

    [JsonProperty(PropertyName = "pages")]
    public Dictionary<string, DataItem> DataItems { get; set; }
}
