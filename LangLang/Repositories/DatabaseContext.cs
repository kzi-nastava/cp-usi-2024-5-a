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
            ConfigureCourseEntity(modelBuilder);
            ConfigureCourseTimeSlotEntity(modelBuilder);
            ConfigureExamSlotEntity(modelBuilder);

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

        private void ConfigureExamSlotEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamSlot>()
            .HasKey(es => es.Id);

            modelBuilder.Entity<ExamSlot>()
                .HasOne<LanguageLevel>()
                .WithMany()
                .HasForeignKey(es => es.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamSlot>()
                .HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(es => es.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExamSlot>()
                .HasOne<TimeSlot>()
                .WithMany()
                .HasForeignKey(es => es.TimeSlotId)
                .OnDelete(DeleteBehavior.Cascade);
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

        private void ConfigureCourseEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Course>()
                .Property(c => c.TutorId)
                .HasColumnName("TutorId");

            modelBuilder.Entity<Course>()
                .Property(c => c.LanguageLevelId)
                .HasColumnName("LanguageLevelId");

            modelBuilder.Entity<Course>()
                .HasOne<Tutor>()
                .WithMany()
                .HasForeignKey(c => c.TutorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Course>()
                .HasOne<LanguageLevel>()
                .WithMany()
                .HasForeignKey(c => c.LanguageLevelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCourseTimeSlotEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseTimeSlot>()
            .HasKey(cts => cts.Id);

            modelBuilder.Entity<CourseTimeSlot>()
                .Property(cts => cts.CourseId)
                .HasColumnName("CourseId");

            modelBuilder.Entity<CourseTimeSlot>()
                .Property(cts => cts.TimeSlotId)
                .HasColumnName("TimeSlotId");

            modelBuilder.Entity<CourseTimeSlot>()
                .HasOne<Course>()
                .WithMany()
                .HasForeignKey(cts => cts.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CourseTimeSlot>()
                .HasOne<TimeSlot>()
                .WithMany()
                .HasForeignKey(cts => cts.TimeSlotId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}