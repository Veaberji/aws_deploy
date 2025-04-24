using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.SimilarArtists;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.TopArtists;

namespace MusiciansAPP.DAL.WebDataProvider;

public class ArtistProfile : Profile
{
    public ArtistProfile()
    {
        CreateMap<LastFmArtistDto, ArtistDAL>();

        CreateMap<LastFmArtistDetailsDto, ArtistDAL>()
            .ForMember(
                dest => dest.Biography,
                o => o.MapFrom(src => src.Biography.Content));

        CreateMap<LastFmSimilarArtistDto, ArtistDAL>();

        CreateMap<LastFmSimilarArtistsDto, SimilarArtistsDAL>()
            .ForMember(
                dest => dest.ArtistName,
                opt => opt.MapFrom(src => src.MetaData.ArtistName))
            .ForMember(
                dest => dest.Artists,
                opt => opt.MapFrom(src => src.Artists));
    }
}