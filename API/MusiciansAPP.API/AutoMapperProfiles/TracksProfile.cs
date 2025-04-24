using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Tracks;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class TracksProfile : Profile
{
    public TracksProfile()
    {
        CreateMap<TrackBL, TrackUI>();
    }
}