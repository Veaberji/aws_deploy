using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MusiciansAPP.DAL.WebDataProvider.ImageProvider.WikipediaDtoModels;
using Newtonsoft.Json;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider;

public class WikipediaDataProvider : IImageProvider
{
    private const string BaseUrl = "https://en.wikipedia.org/w/api.php";
    private readonly IHttpClient _httpClient;

    public WikipediaDataProvider(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetArtistImageUrlAsync(string name)
    {
        const string defaultArtistImage = "https://i.ibb.co/6H89Zzh/default-artist.jpg";

        var response = await GetArtistImageResponseAsync(name);
        var content = await GetResponseContentAsync(response);
        var result = JsonConvert.DeserializeObject<ImageResponseTopLevel>(content);

        return result.Query.DataItems.Values.FirstOrDefault()?.Image?.ImageUrl ?? defaultArtistImage;
    }

    private static async Task<string> GetResponseContentAsync(HttpResponseMessage response)
    {
        return await response.Content.ReadAsStringAsync();
    }

    private async Task<HttpResponseMessage> GetArtistImageResponseAsync(string name)
    {
        const string action = "query";
        const string property = "pageimages";
        const string imageSize = "original";

        var url = $"{BaseUrl}?action={action}&titles={name}&prop={property}&piprop={imageSize}&format=json";

        return await GetResponseAsync(url);
    }

    private async Task<HttpResponseMessage> GetResponseAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }
}