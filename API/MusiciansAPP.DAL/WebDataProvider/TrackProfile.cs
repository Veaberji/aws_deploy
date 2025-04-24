using AutoMapper;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.AlbumDetails;
using MusiciansAPP.DAL.WebDataProvider.LastFmDtoModels.ArtistTopTracks;

namespace MusiciansAPP.DAL.WebDataProvider;

public class TrackProfile : Profile
{
    public TrackProfile()
    {
        CreateMap<LastFmArtistTopTracksDto, ArtistTracksDAL>()
            .ForMember(
                dest => dest.ArtistName,
                o => o.MapFrom(src => src.MetaData.ArtistName))
            .ForMember(
                dest => dest.Tracks,
                o => o.MapFrom(src => src.Tracks));

        CreateMap<LastFmArtistTopTrackDto, TrackDAL>()
            .ForMember(
                dest => dest.PlayCount,
                o => o.MapFrom(src => MapData(src.PlayCount)));

        CreateMap<LastFmAlbumTrackDto, TrackDAL>()
            .ForMember(
                dest => dest.DurationInSeconds,
                o => o.MapFrom(src => MapData(src.DurationInSeconds)));
    }

    private static int MapData(int? data)
    {
        // data (PlayCount or DurationInSeconds) is null in this point means last.fm doesn't have it
        return data ?? 0;
    }
}