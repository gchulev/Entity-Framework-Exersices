using Microsoft.EntityFrameworkCore;

using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Homework> Homeworks { get; set; } = null!;
        public virtual DbSet<Resource> Resources { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=StudentSystem;Integrated Security=True;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(p => p.StudentId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();

                entity.Property(p => p.Name)
                    .HasColumnType("nvarchar(100)")
                    .IsRequired();

                entity.Property(p => p.PhoneNumber)
                    .HasColumnType("varchar(10)")
                    .IsFixedLength();

                entity.Property(p => p.RegisteredOn)
                    .IsRequired();

                entity.HasMany(s => s.Homeworks)
                    .WithOne(h => h.Student)
                    .HasForeignKey(h => h.StudentId);

            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.Property(p => p.CourseId)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();

                entity.Property(p => p.Name)
                    .HasColumnType("nvarchar(80)")
                    .IsUnicode();

                entity.Property(p => p.Description)
                    .HasColumnType("nvarchar(max)");

                entity.Property(p => p.StartDate)
                    .IsRequired()
                    .HasColumnType("datetime2");

                entity.Property(p => p.EndDate) 
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(p => p.Price)
                    .IsRequired();

                entity.HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(r => r.CourseId);

                entity.HasMany(c => c.Homeworks)
                    .WithOne(h => h.Course)
                    .HasForeignKey(h => h.CourseId);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(e => e.ResourceId);

                entity.Property(p => p.ResourceId)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

                entity.Property(p => p.Name)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

                entity.Property(p => p.Url)
                .HasColumnType("varchar(255)")
                .IsRequired();

                entity.Property(p => p.ResourceType)
                .IsRequired();

            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(e => e.HomeworkId);

                entity.Property(p => p.Content)
                .HasColumnType("varchar(max)")
                .IsRequired();

                entity.Property(p => p.ContentType)
                .IsRequired();

                entity.Property(p => p.SubmissionTime)
                .HasColumnType("datetime2")
                .IsRequired();

                entity.HasOne(s => s.Student)
                .WithMany(h => h.Homeworks)
                .HasForeignKey(h => h.StudentId);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(k => new { k.StudentId, k.CourseId });

                entity.HasOne(sc => sc.Student)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.StudentId);

                entity.HasOne(sc => sc.Course)
                .WithMany(c => c.StudentsCourses)
                .HasForeignKey(sc => sc.CourseId);
            });
        }
    }
}
