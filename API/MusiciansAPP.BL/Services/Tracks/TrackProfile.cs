using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Tracks;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<TrackDAL, TrackBL>();
        CreateMap<TrackBL, Track>().ReverseMap();

        CreateMap<ArtistTracksDAL, ArtistTracksBL>();
    }
}