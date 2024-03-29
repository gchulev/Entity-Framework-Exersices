﻿namespace SoftJail.Data
{
    using Microsoft.EntityFrameworkCore;

    using SoftJail.Data.Models;

    public class SoftJailDbContext : DbContext
    {
        public SoftJailDbContext()
        {
        }

        public SoftJailDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Cell> Cells { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Mail> Mails { get; set; } = null!;
        public virtual DbSet<Officer> Officers { get; set; } = null!;
        public virtual DbSet<Prisoner> Prisoners { get; set; } = null!;
        public virtual DbSet<OfficerPrisoner> OfficersPrisoners { get; set; } = null!;

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
            builder.Entity<OfficerPrisoner>(entity =>
            {
                entity.HasKey(k => new { k.OfficerId, k.PrisonerId});

                entity.HasOne(op => op.Officer)
                .WithMany(o => o.OfficerPrisoners)
                .HasForeignKey(op => op.OfficerId)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(op => op.Prisoner)
                .WithMany(p => p.PrisonerOfficers)
                .HasForeignKey(op => op.PrisonerId)
                .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}