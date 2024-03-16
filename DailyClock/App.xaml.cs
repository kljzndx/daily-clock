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
            .AddSingleton(GetConfig)
            .AddSingleton<MainViewModel>()
            .BuildServiceProvider();

            Ioc.Default.ConfigureServices(series);
        }

        private AppSettings GetConfig(IServiceProvider services)
        {
            string filePath = "./appsettings.json";
            var logger = services.GetRequiredService<ILogger<AppSettings>>();

            logger.LogInformation("读取配置文件");

            AppSettings? __result = null;
            try
            {
                string json = File.ReadAllText(filePath);
                __result = JsonSerializer.Deserialize<AppSettings>(json);
            }
            catch (FileNotFoundException) 
            {
                logger.LogError("未找到配置文件，将使用默认配置");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "读取配置文件失败，将使用默认配置");
            }

            AppSettings result = __result ?? new();

            result.PropertyChanged += async (s, e) =>
            {
                if (s is not AppSettings the)
                    return;

                var l = Ioc.Default.GetRequiredService<ILogger<AppSettings>>();
                l.LogInformation("保存配置文件");

                try
                {
                    await File.WriteAllTextAsync(filePath, the.ToString());
                }
                catch (Exception ex)
                {
                    l.LogError(ex, "保存配置文件失败");
                }
            };

            return result;
        }
    }

}
