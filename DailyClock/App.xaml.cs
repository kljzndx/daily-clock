using CommunityToolkit.Mvvm.DependencyInjection;

using DailyClock.Models;
using DailyClock.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace DailyClock
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var series = new ServiceCollection()
            .AddLogging(lb =>
            {
                var log = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory, "main.log"), fileSizeLimitBytes: 10240)
                .CreateLogger();
                lb.AddSerilog(log);
            })
            .AddSingleton(s => GetConfig())
            .AddSingleton<MainViewModel>()
            .BuildServiceProvider();

            Ioc.Default.ConfigureServices(series);
        }

        private AppSettings GetConfig()
        {
            string filePath = "./appsettings.json";

            AppSettings? __result = null;
            try
            {
                string json = File.ReadAllText(filePath);
                __result = JsonSerializer.Deserialize<AppSettings>(json);
            }
            catch (Exception ex)
            {

            }

            AppSettings result = __result ?? new();

            result.PropertyChanged += (s, e) =>
            {
                if (s is not AppSettings the)
                    return;

                File.WriteAllText(filePath, the.ToString());
            };

            return result;
        }
    }

}
