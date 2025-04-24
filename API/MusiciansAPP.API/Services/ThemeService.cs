using System;

namespace MusiciansAPP.API.Services;

public class ThemeService : IThemeService
{
    private readonly IThemeDataProvider _theme;

    public ThemeService(IThemeDataProvider theme)
    {
        _theme = theme;
    }

    public bool IsDarkTheme(DateTime date)
    {
        var time = TimeOnly.FromDateTime(date);
        var darkFromTime = TimeOnly.FromDateTime(_theme.DarkFromTime);
        var darkToTime = TimeOnly.FromDateTime(_theme.DarkToTime);

        return time.IsBetween(darkFromTime, darkToTime);
    }
}
