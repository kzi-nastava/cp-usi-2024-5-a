using LangLang.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LangLang.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<ExamSlot> ExamSlot { get; set; }
        public DbSet<LanguageLevel> LanguageLevel { get; set; }
        public DbSet<CourseTimeSlot> CourseTimeSlot { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<ExamSlot>().ToTable("ExamSlot");
            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot");
            modelBuilder.Entity<LanguageLevel>().ToTable("LanguageLevel");
            modelBuilder.Entity<CourseTimeSlot>().ToTable("CourseTimeSlot");

        }
    }
}