using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Artists;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<ArtistDAL, ArtistBL>();
        CreateMap<ArtistBL, Artist>().ReverseMap();

        CreateMap<SimilarArtistsDAL, SimilarArtistsBL>();
        CreateMap<SimilarArtistsBL, Artist>()
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.ArtistName));
    }
}