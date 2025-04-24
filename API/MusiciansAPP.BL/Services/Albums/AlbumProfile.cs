using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.Domain;

namespace MusiciansAPP.BL.Services.Albums;

public class AlbumProfile : Profile
{
    public AlbumProfile()
    {
        CreateMap<AlbumDAL, AlbumBL>();
        CreateMap<AlbumBL, Album>();
        CreateMap<Album, AlbumBL>()
            .ForMember(
                dest => dest.ArtistName,
                opt => opt.MapFrom(src => src.Artist.Name));

        CreateMap<ArtistAlbumsDAL, ArtistAlbumsBL>();
    }
}