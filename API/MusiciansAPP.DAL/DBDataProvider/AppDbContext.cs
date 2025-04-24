using Microsoft.EntityFrameworkCore;
using MusiciansAPP.Domain;

namespace MusiciansAPP.DAL.DBDataProvider;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Artist> Artists { get; set; }

    public DbSet<Album> Albums { get; set; }

    public DbSet<Track> Tracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>()
            .HasMany(a => a.SimilarArtists)
            .WithMany(a => a.ReverseSimilarArtists)
            .UsingEntity(e => e.ToTable("SimilarArtists"));

        modelBuilder.Entity<Album>(builder =>
        {
            builder.Property(a => a.DateCreated)
                .HasConversion<DateOnlyConverter>();
        });
    }
}