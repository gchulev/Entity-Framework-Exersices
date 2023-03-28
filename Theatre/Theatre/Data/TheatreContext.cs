
namespace Theatre.Data
{
    using Microsoft.EntityFrameworkCore;

    using Theatre.Data.Models;

    public class TheatreContext : DbContext
    {
        public TheatreContext() 
        {
        }

        public TheatreContext(DbContextOptions options)
        : base(options) 
        { 
        }

        public virtual DbSet<Cast> Casts { get; set; } = null!;
        public virtual DbSet<Play> Plays { get; set; } = null!;
        public virtual DbSet<Theatre> Theatres { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Play>(entity =>
            {
                entity.HasCheckConstraint("CK_Play_Duration", "[Duration] >= '01:00:00'");
            });
        }
    }
}