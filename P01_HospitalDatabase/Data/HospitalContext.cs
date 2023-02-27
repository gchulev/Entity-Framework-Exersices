using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Diagnose> Diagnoses { get; set; } = null!;
        public DbSet<Medicament> Medicaments { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<Visitation> Visitations { get; set; } = null!;
        public DbSet<PatientMedicament> PatientsMedicaments { get; set; } = null!;
        public DbSet<Doctor> Doctors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Hospital;Integrated Security=True;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(p => p.FirstName)
                .IsUnicode()
                .HasColumnType("NVARCHAR(50)");

                entity.Property(p => p.LastName)
                .IsUnicode()
                .HasColumnType("NVARCHAR(50)");

                entity.Property(p => p.Address)
                .IsUnicode()
                .HasColumnType("NCARCHAR(250)");

                entity.Property(p => p.Email)
                .HasColumnType("VARCHAR(80)");

                entity.HasKey(e => e.PatientId);

                entity.HasKey(p => p.PatientId);

                entity.HasMany(p => p.Visitations)
                .WithOne(v => v.Patient)
                .HasForeignKey(v => v.PatientId);//check if this is correct relation

                entity.HasOne(p => p.Diagnose)
                .WithMany(d => d.Patients)
                .HasForeignKey(d => d.PatientId);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.Property(p => p.Comments)
                .HasColumnType("NVARCHAR(250)");

                entity.HasKey(v => v.VisitationId);

                entity.HasOne(v => v.Doctor)
                .WithMany(d => d.Visitations)
                .HasForeignKey(v => v.DoctorId);
            });

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.Property(p => p.Name)
                .HasColumnType("NVARCHAR(50)");

                entity.Property(p => p.Comments)
                .HasColumnType("NVARCHAR(250)");

                entity.HasKey(d => d.DiagnoseId);
            });

            modelBuilder.Entity<Medicament>(entity =>
            {
                entity.Property(p => p.Name)
                .HasColumnType("NVARCHAR(50)");

                entity.HasKey(m => m.MedicamentId);
            });

            modelBuilder.Entity<PatientMedicament>(entity =>
            {
                entity.HasKey(k => new { k.PatientId, k.MedicamentId});

                entity.HasOne(pm => pm.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientId);

                entity.HasOne(pm => pm.Medicament)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(m => m.MedicamentId);
            });
    }
}
}
