using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudentManagement.Web.Models;

public partial class StudentDbContext : DbContext
{
    private readonly IConfiguration? _configuration = null;

    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Connection string'i appsettings.json'dan oku
            var connectionString = _configuration != null
                ? _configuration.GetConnectionString("DefaultConnection")
                : "Server=(localdb)\\mssqllocaldb;Database=StudentDB;Trusted_Connection=True;";
            
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D71A7CAC55DF8");

            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.Credits).HasDefaultValue(3);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B999658FB5C");

            entity.HasIndex(e => e.Email, "UQ__Students__A9D10534A8A3F2F4").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
        });

        modelBuilder.Entity<StudentCourse>(entity =>
        {
            entity.HasKey(e => e.StudentCourseId).HasName("PK__StudentC__7E3E2F92C2E0B3E9");

            entity.HasIndex(e => new { e.StudentId, e.CourseId }, "UQ__StudentC__5E57FC828B4E4BDD").IsUnique();

            entity.Property(e => e.EnrollmentDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Grade).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Course).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__StudentCo__Cours__2F10007B");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentCourses)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__StudentCo__Stude__2E1BDC42");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
