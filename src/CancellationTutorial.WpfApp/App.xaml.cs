using System.Windows;
using CancellationTutorial.CoreLib;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CancellationTutorial.WpfApp
{
    public partial class App : Application
    {
        private readonly IHost host;

        public App()
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(context.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureLogging((context, builder) =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddTransient<IRepository, Repository>();
                    services.AddHttpClient();
                })
                .Build();
            
            Ioc.Default.ConfigureServices(host.Services);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();

            MainWindow = Ioc.Default.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
        }
    }
}
