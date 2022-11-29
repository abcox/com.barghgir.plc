using com.barghgir.plc.data.Context;
using Microsoft.EntityFrameworkCore;

namespace com.barghgir.plc.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Content> Content { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Course> CourseContent { get; set; }
        public DbSet<Member> Member { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>().ToTable(nameof(Content));
            modelBuilder.Entity<Course>().ToTable(nameof(Course));
            modelBuilder.Entity<CourseContent>().ToTable(nameof(CourseContent));
            modelBuilder.Entity<Member>().ToTable(nameof(Member));
        }
    }
}