using System.Net.Http;
using System.Threading.Tasks;

namespace MusiciansAPP.DAL.WebDataProvider;

public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string url);
}