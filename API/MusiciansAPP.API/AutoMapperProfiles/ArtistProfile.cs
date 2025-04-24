using AutoMapper;
using MusiciansAPP.API.UIModels;
using MusiciansAPP.BL.Services.Artists;

namespace MusiciansAPP.API.AutoMapperProfiles;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<ArtistBL, ArtistUI>();
    }
}