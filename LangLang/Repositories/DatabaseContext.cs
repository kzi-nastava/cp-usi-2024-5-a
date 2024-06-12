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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Shadow property for CourseId
            modelBuilder.Entity<TimeSlot>()
                .Property<int?>("CourseId");

            // Shadow property for ExamId
            modelBuilder.Entity<TimeSlot>()
                .Property<int?>("ExamId");

            // Configure the relationship between TimeSlot and Course
            modelBuilder.Entity<TimeSlot>()
                .HasOne<Course>()
                .WithMany(c => c.TimeSlots)
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between TimeSlot and Exam
            modelBuilder.Entity<TimeSlot>()
                .HasOne<ExamSlot>()
                .WithOne(e => e.TimeSlot)
                .HasForeignKey<TimeSlot>("ExamId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<ExamSlot>().ToTable("ExamSlot");
            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot");

        }
    }
}