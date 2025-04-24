using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusiciansAPP.API.Exceptions;
using MusiciansAPP.API.Services;
using MusiciansAPP.BL.Exceptions;
using MusiciansAPP.DAL.Exceptions;

namespace MusiciansAPP.API.Controllers;

[ApiController]
public abstract class AppController : ControllerBase
{
    private readonly IErrorHandler _errorHandler;

    protected AppController(IErrorHandler errorHandler)
    {
        _errorHandler = errorHandler;
    }

    protected ActionResult<T> GetData<T>(Func<T> func, string method)
    {
        try
        {
            return Ok(func());
        }
        catch (NotAllowedException error)
        {
            return NotFound(error.Message);
        }
        catch (Exception error)
        {
            _errorHandler.HandleError(error, method);
            return CreateDefaultError();
        }
    }

    protected async Task<ActionResult<T>> GetDataAsync<T>(Func<Task<T>> func, string method)
    {
        try
        {
            return Ok(await func());
        }
        catch (NotFoundException error)
        {
            return NotFound(error.Message);
        }
        catch (DataServiceUnavailableException error)
        {
            _errorHandler.HandleError(error, method);
            return CreateDataServiceUnavailableError();
        }
        catch (Exception error)
        {
            _errorHandler.HandleError(error, method);
            return CreateDefaultError();
        }
    }

    protected async Task<ActionResult> GetFileAsync(Func<Task<FileContentResult>> func, string method)
    {
        try
        {
            return await func();
        }
        catch (DataServiceUnavailableException error)
        {
            _errorHandler.HandleError(error, method);
            return CreateDataServiceUnavailableError();
        }
        catch (OperationCanceledException)
        {
            return BadRequest("Request was canceled");
        }
        catch (Exception error)
        {
            _errorHandler.HandleError(error, method);
            return CreateDefaultError();
        }
    }

    private ObjectResult CreateDefaultError()
    {
        return StatusCode(
            StatusCodes.Status500InternalServerError,
            "A problem happened while handling your request.");
    }

    private ObjectResult CreateDataServiceUnavailableError()
    {
        return StatusCode(StatusCodes.Status503ServiceUnavailable, "Data Service is Unavailable");
    }
}