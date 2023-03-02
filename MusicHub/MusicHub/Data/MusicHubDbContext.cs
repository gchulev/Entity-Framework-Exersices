namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;

    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; } = null!;
        public DbSet<Performer> Performers { get; set; } = null!;
        public DbSet<Producer> Producers { get; set; } = null!;
        public DbSet<Song> Songs { get; set; } = null!;
        public DbSet<Writer> Writers { get; set; } = null!;
        public DbSet<SongPerformer> SongsPerformers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SongPerformer>(entity =>
            {
                entity.HasKey(sp => new { sp.SongId, sp.PerformerId });

                entity.HasOne(sp => sp.Song)
                .WithMany(s => s.SongPerformers)
                .HasForeignKey(sp => sp.SongId);

                entity.HasOne(sp => sp.Performer)
                .WithMany(s => s.PerformerSongs)
                .HasForeignKey(sp => sp.PerformerId);
            });

            builder.Entity<Song>(entity =>
            {
                entity.Property(s => s.Name)
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(s => s.Duration)
                .IsRequired();

                entity.Property(s => s.CreatedOn)
                .IsRequired();

                entity.Property(s => s.Genre)
                .IsRequired();

                entity.Property(s => s.WriterId)
                .IsRequired();

                entity.Property(s => s.Price)
                .IsRequired();

                entity.HasKey(s => s.Id);

                entity.HasOne(s => s.Writer)
                .WithMany(w => w.Songs)
                .HasForeignKey(s => s.WriterId);

                entity.HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId);
            });

            builder.Entity<Album>(entity =>
            {
                entity.Property(a => a.Name)
                .HasMaxLength(40)
                .IsRequired();

                entity.Property(a => a.ReleaseDate)
                .IsRequired();

                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.Producer)
                .WithMany(p => p.Albums)
                .HasForeignKey(a => a.ProducerId);
            });

            builder.Entity<Performer>(entity =>
            {
                entity.Property(p => p.FirstName)
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(p => p.LastName)
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(p => p.Age)
                .IsRequired();

                entity.Property(p => p.NetWorth)
                .IsRequired();

                entity.HasKey(p => p.Id);
            });

            builder.Entity<Producer>(entity =>
            {
                entity.Property(pr => pr.Name)
                .HasMaxLength(30)
                .IsRequired();

                entity.HasKey(pr => pr.Id);
            });

            builder.Entity<Writer>(entity =>
            {
                entity.Property(w => w.Name)
                .HasMaxLength(20)
                .IsRequired();

                entity.Property(w => w.Pseudonym);

                entity.HasKey(w => w.Id);
            });
        }
    }
}
