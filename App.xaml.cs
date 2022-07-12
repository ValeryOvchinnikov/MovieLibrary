using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieLibrary.DBContext;
using MovieLibrary.Models;
using MovieLibrary.Repositories;
using MovieLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MovieLibrary
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
       
        public App()
        {
            host = Host.CreateDefaultBuilder().ConfigureAppConfiguration(hostConfig =>
            {
            }).ConfigureServices((context, services) =>
            {
                ConfigureServices(context.Configuration, services);
            }).Build();
        }
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            
            await host.StartAsync();
            var mainWindow = host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }

        protected void ConfigureServices(IConfiguration configuration,IServiceCollection services)
        {
            services.AddTransient<IRepository<Movie>, MovieRepository>();
            services.AddTransient<IRepository<Director>, DirectorRepository>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddSingleton<MainWindow>();
        }

        protected async void Application_Exit(ExitEventArgs e)
        {
            using (host)
            {
                await host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
