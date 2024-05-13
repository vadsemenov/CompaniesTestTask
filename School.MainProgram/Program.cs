using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using School.DataAccess.Model;
using School.DataAccess.UnitOfWork;
using School.MainProgram.View;
using School.MainProgram.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace School.MainProgram
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    
                    services.AddSingleton<SchoolViewModel>();

                    services.AddSingleton<MainWindow>();
                    services.AddTransient<AddStudentWindow>();
                    services.AddTransient<AddSubjectWindow>();
                    services.AddTransient<AddAssessmentWindow>();

                    services.AddDbContext<SchoolDbContext>();

                    services.AddSingleton<IUnitOfWork>(p =>
                    {
                        //Можно просто создать new SchoolDbContext
                        var dbContext = p.GetRequiredService<SchoolDbContext>();

                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();

                        return new UnitOfWork(dbContext);
                    });
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
