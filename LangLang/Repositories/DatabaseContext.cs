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
        public DbSet<Skill> Skill { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TimeSlot>()
                .Property<int?>("CourseId");

            modelBuilder.Entity<TimeSlot>()
                .Property<int?>("ExamId");

            modelBuilder.Entity<TimeSlot>()
                .HasOne<Course>()
                .WithMany(c => c.TimeSlots)
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TimeSlot>()
                .HasOne<ExamSlot>()
                .WithOne(e => e.TimeSlot)
                .HasForeignKey<TimeSlot>("ExamId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<ExamSlot>().ToTable("ExamSlot");
            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot");
            modelBuilder.Entity<Skill>().ToTable("Skill");
        }
    }
}