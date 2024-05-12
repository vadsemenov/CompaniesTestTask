using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.DependencyInjection;
using School.DataAccess.Model;
using School.MainProgram.View;
using School.MainProgram.ViewModel;

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
                })
                .Build();

            var app = host.Services.GetService<App>();

            app?.Run();
        }
    }
}
