using System;
using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Albums;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class AlbumsProfile : Profile
{
    public AlbumsProfile()
    {
        CreateMap<AlbumBL, AlbumUI>().ForMember(
            dest => dest.DateCreated,
            opt => opt.MapFrom(src => src.DateCreated.ToDateTime(TimeOnly.MinValue)));
    }
}