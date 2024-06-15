using LangLang.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LangLang.Repositories
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<ExamSlot> ExamSlot { get; set; }
        public DbSet<LanguageLevel> LanguageLevel { get; set; }
        public DbSet<CourseTimeSlot> CourseTimeSlot { get; set; }   
        public DbSet<TutorSkill> TutorSkill {  get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureTutorEntity(modelBuilder);
            ConfigureTutorSkillEntity(modelBuilder);

            modelBuilder.Entity<Tutor>().ToTable("Tutor");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<ExamSlot>().ToTable("ExamSlot");
            modelBuilder.Entity<TimeSlot>().ToTable("TimeSlot");
            modelBuilder.Entity<LanguageLevel>().ToTable("LanguageLevel");
            modelBuilder.Entity<CourseTimeSlot>().ToTable("CourseTimeSlot");
            modelBuilder.Entity<TutorSkill>().ToTable("TutorSkill");
        }

        private void ConfigureTutorEntity(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Tutor>()
                .OwnsOne(t => t.Profile, profile =>
                {
                    profile.Property(p => p.Name).HasColumnName("Name");
                    profile.Property(p => p.LastName).HasColumnName("LastName");
                    profile.Property(p => p.Gender).HasColumnName("Gender");
                    profile.Property(p => p.BirthDate).HasColumnName("BirthDate");
                    profile.Property(p => p.PhoneNumber).HasColumnName("PhoneNumber");
                    profile.Property(p => p.Email).HasColumnName("Email");
                    profile.Property(p => p.Password).HasColumnName("Password");
                    profile.Property(p => p.Role).HasColumnName("Role");
                    profile.Property(p => p.IsActive).HasColumnName("IsActive");
                });
        }

        private void ConfigureTutorSkillEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TutorSkill>()
                .HasKey(ts => ts.Id);

            modelBuilder.Entity<TutorSkill>()
                .HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(ts => ts.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TutorSkill>()
                .HasOne<LanguageLevel>()
                .WithMany()
                .HasForeignKey(ts => ts.LanguageLevelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}