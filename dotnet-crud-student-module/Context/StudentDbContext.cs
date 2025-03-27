using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCrudStarter.StudentModule.Entities;

namespace NetCrudStarter.StudentModule.Context;

public partial class StudentDbContext : DbContext
{
    public StudentDbContext()
    {
    }

    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Enrolment> Enrolments { get; set; }
    
    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {



        modelBuilder.Entity<Enrolment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("enrolment_pkey");

            entity.ToTable("enrolment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id");
            entity.Property(e => e.EnrolmentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("enrolment_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            
            entity.HasOne(d => d.Student).WithMany(p => p.Enrolments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("enrolment_student_id_fkey");
        });



        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_pkey");

            entity.ToTable("student");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birth_date");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.SchoolId).HasColumnName("school_id");
            
        });

        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
