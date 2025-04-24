using System;
using MusiciansAPP.API.Configs;

namespace MusiciansAPP.API.Services;

public class ThemeDataProvider : IThemeDataProvider
{
    public DateTime DarkFromTime => Theme.DarkFromTime;

    public DateTime DarkToTime => Theme.DarkToTime;
}