using System;
using System.Net.Http;
using System.Threading.Tasks;
using MusiciansAPP.BL.Exceptions;

namespace MusiciansAPP.BL.Services;

public class BaseService
{
    protected async Task<T> GetDataAsync<T>(Func<Task<T>> getData, Func<Task<T>> onDbException = null)
    {
        try
        {
            return await getData();
        }
        catch (InvalidOperationException error)
        {
            return onDbException is not null
                ? await GetDataFromSecondSource(onDbException)
                : throw new DataServiceUnavailableException("DB is Unavailable", error);
        }
        catch (HttpRequestException error)
        {
            throw new DataServiceUnavailableException("WebDataProvider is Unavailable", error);
        }
    }

    private async Task<T> GetDataFromSecondSource<T>(Func<Task<T>> getData)
    {
        try
        {
            return await getData();
        }
        catch (HttpRequestException error)
        {
            throw new DataServiceUnavailableException("WebDataProvider is Unavailable", error);
        }
    }
}
