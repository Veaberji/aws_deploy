using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusiciansAPP.Domain.Constraints;

namespace MusiciansAPP.Domain;

public class Artist : Entity
{
    public Artist()
    {
        Tracks = new List<Track>();
        Albums = new List<Album>();
        SimilarArtists = new List<Artist>();
        ReverseSimilarArtists = new List<Artist>();
    }

    public Guid Id { get; set; }

    [Required]
    [MaxLength(ArtistConstraints.NameMaxLength)]
    public string Name { get; set; }

    [MaxLength(ArtistConstraints.ImageUrlMaxLength)]
    public string ImageUrl { get; set; }

    [Column(TypeName = "ntext")]
    public string Biography { get; set; }

    public IEnumerable<Track> Tracks { get; set; }

    public IEnumerable<Album> Albums { get; set; }

    [InverseProperty(nameof(ReverseSimilarArtists))]
    public List<Artist> SimilarArtists { get; set; }

    [InverseProperty(nameof(SimilarArtists))]
    public IEnumerable<Artist> ReverseSimilarArtists { get; set; }

    public bool IsArtistDetailsUpToDate()
    {
        return IsArtistHasImageUrl() && !string.IsNullOrWhiteSpace(Biography);
    }

    public bool IsArtistHasImageUrl()
    {
        return !string.IsNullOrWhiteSpace(ImageUrl);
    }

    protected override bool IsFull()
    {
        return IsArtistHasImageUrl();
    }
}