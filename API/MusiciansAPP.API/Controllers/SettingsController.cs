using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MusiciansAPP.API.Exceptions;
using MusiciansAPP.API.Services;
using MusiciansAPP.API.UIModels;

namespace MusiciansAPP.API.Controllers;

[Route("api/settings")]
public class SettingsController : AppController
{
    private readonly IHostEnvironment _environment;
    private readonly IThemeService _themeService;

    public SettingsController(
        IErrorHandler errorHandler,
        IHostEnvironment environment,
        IThemeService themeService)
        : base(errorHandler)
    {
        _environment = environment;
        _themeService = themeService;
    }

    [HttpGet("isDark/{date}")]
    public ActionResult<ThemeSettings> IsDarkTheme(DateTime date)
    {
        var func = () =>
        {
            if (!_environment.IsProduction())
            {
                throw new NotAllowedException("It's Available only in Production");
            }

            return new ThemeSettings { IsDarkTheme = _themeService.IsDarkTheme(date) };
        };

        return GetData(func, nameof(IsDarkTheme));
    }
}
