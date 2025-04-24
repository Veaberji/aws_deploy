using System;
using System.ComponentModel.DataAnnotations;
using MusiciansAPP.Domain.Constraints;

namespace MusiciansAPP.Domain;

public class Track : Entity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(TrackConstraints.NameMaxLength)]
    public string Name { get; set; }

    [Range(TrackConstraints.PlayCountMinValue, int.MaxValue)]
    public int? PlayCount { get; set; }

    [Range(TrackConstraints.DurationInSecondsMinValue, int.MaxValue)]
    public int? DurationInSeconds { get; set; }

    public Artist Artist { get; set; }

    public Album Album { get; set; }

    public bool IsTrackHasPlayCount()
    {
        return PlayCount is not null;
    }

    public bool IsTrackHasDurationInSeconds()
    {
        return DurationInSeconds is not null;
    }

    protected override bool IsFull()
    {
        return IsTrackHasPlayCount();
    }
}