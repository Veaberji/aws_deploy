using System;

namespace MusiciansAPP.API.Services;

public interface IThemeDataProvider
{
    DateTime DarkFromTime { get; }

    DateTime DarkToTime { get; }
}