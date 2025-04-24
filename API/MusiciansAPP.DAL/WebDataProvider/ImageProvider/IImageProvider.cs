using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider.ImageProvider;

public interface IImageProvider
{
    Task<string> GetArtistImageUrlAsync(string name);
}