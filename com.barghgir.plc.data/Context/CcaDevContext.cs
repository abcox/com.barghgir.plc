using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace com.barghgir.plc.data.Context;

public partial class CcaDevContext : DbContext
{
    public CcaDevContext()
    {
    }

    public CcaDevContext(DbContextOptions<CcaDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseContent> CourseContents { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        // see Program.cs configuration during startup
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Content__3214EC07397F5020");

            entity.ToTable("Content");

            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course__3214EC0794181FCA");

            entity.ToTable("Course");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.Subtitle).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<CourseContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CourseCo__3214EC070892D7CF");

            entity.ToTable("CourseContent");

            entity.HasOne(d => d.Content).WithMany(p => p.CourseContents)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("FK_CourseContent_Content");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseContents)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK_CourseContent_Course");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Member__3214EC07B7B6F2C7");

            entity.ToTable("Member");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsAdmin).HasDefaultValueSql("((1))");
            entity.Property(e => e.LastFailedSignInDate).HasColumnType("datetime");
            entity.Property(e => e.LastPasswordUpdate).HasColumnType("datetime");
            entity.Property(e => e.LastSignInDate).HasColumnType("datetime");
            entity.Property(e => e.LockDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VerifyCode)
                .HasMaxLength(6)
                .IsFixedLength();
            entity.Property(e => e.VerifyDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
