using CommonModel;
using Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Batch_Rename_App
{
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public IConfiguration _Configuration { get; private set; }
        public App()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var Config = new ConfigurationBuilder();
            Config.SetBasePath(currentDirectory);

            var staticConfigDirectory = Path.Combine(currentDirectory, "StaticConfig");
            var filePaths = Directory.GetFiles(staticConfigDirectory, "*.json", SearchOption.TopDirectoryOnly);
            List<string> configUrls = new List<string>();
            var staticConfigUrls = new List<string>();
            foreach (var file in filePaths)
            {
                var fileName = Path.GetFileName(file);
                staticConfigUrls.Add(Path.Combine(@$"StaticConfig", fileName));
            }
            configUrls.AddRange(staticConfigUrls);

            foreach (var url in configUrls)
            {
                if (File.Exists(url))
                {
                    Config.AddJsonFile(url, optional: false, reloadOnChange: true);
                }
            }
            _Configuration = Config.Build();
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<RuleConfig>(_Configuration.GetSection("RuleConfig"));
                    services.AddSingleton<MainWindow>();
                }).Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost?.StartAsync();
            var startupWindow = AppHost.Services.GetRequiredService<MainWindow>();
            startupWindow.Show();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost?.StopAsync();

            base.OnExit(e);
        }
    }
}
