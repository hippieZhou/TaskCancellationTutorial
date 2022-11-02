﻿using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CoreLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrondendApp
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
                .ConfigureServices((context, collection) => { collection.AddHttpClient<MyHttpClient>(); })
                .Build();
            
            Ioc.Default.ConfigureServices(host.Services);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();
        }
    }
}