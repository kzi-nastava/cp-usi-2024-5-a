using LangLang.Composition;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace LangLang
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            DotNetEnv.Env.Load();

            _host = Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
                {
                    var host = Environment.GetEnvironmentVariable("HOST");
                    var database = Environment.GetEnvironmentVariable("DATABASE");
                    var username = Environment.GetEnvironmentVariable("USERNAME");
                    var password = Environment.GetEnvironmentVariable("PASSWORD");

                    var connectionString = $"Host={host};Database={database};Username={username};Password={password}";

                    services.AddDbContext<DatabaseContext>(options =>
                        options.UseNpgsql(connectionString));
                    services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();
                    services.AddTransient<ICourseRepository, CourseRepository>();
                    services.AddTransient<IExamSlotRepository, ExamSlotRepository>();
                    services.AddTransient<ILanguageLevelRepository, LanguageLevelRepository>();
                    services.AddTransient<ITutorRepository, TutorRepository>();
                    services.AddTransient<ITutorSkillRepository, TutorSkillRepository>();
                    
                }).Build();

            Injector.SetServiceProvider(_host.Services as ServiceProvider);
            ApplyMigrations();
        }

        private void ApplyMigrations()
        {
            using (var scope = _host.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync().Wait();
            base.OnExit(e);
        }

    }

}