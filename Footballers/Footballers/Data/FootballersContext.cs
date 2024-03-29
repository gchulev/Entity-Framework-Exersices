﻿namespace Footballers.Data
{
    using Footballers.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class FootballersContext : DbContext
    {
        public FootballersContext() { }

        public FootballersContext(DbContextOptions options)
            : base(options) { }


        public virtual DbSet<Coach> Coaches { get; set; } = null!;
        public virtual DbSet<Footballer> Footballers { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;
        public virtual DbSet<TeamFootballer> TeamsFootballers { get; set; } = null!;

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
            modelBuilder.Entity<TeamFootballer>(entity =>
            {
                entity.HasKey(k => new { k.FootballerId, k.TeamId });

                entity.HasOne(tf => tf.Team)
                .WithMany(t => t.TeamsFootballers)
                .HasForeignKey(tf => tf.TeamId);

                entity.HasOne(tf => tf.Footballer)
                .WithMany(f => f.TeamsFootballers)
                .HasForeignKey(tf => tf.FootballerId);
            });
        }
    }
}
