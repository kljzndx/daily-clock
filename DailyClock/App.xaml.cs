using CommunityToolkit.Mvvm.DependencyInjection;

using DailyClock.Models;
using DailyClock.Models.Tones;
using DailyClock.Services;
using DailyClock.Services.Tables;
using DailyClock.ViewModels;
using DailyClock.Views;

using FreeSql;

using H.NotifyIcon;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Configuration;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Navigation;

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
            .AddSingleton(s => GetConfig(s.GetRequiredService<ILogger<AppSettings>>()))
            .AddSingleton(s =>
            {
                var logger = s.GetRequiredService<ILogger<App>>();
                
                logger.LogInformation("读取数据库");

                var fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, @"Data Source=main.db;")
                .UseAutoSyncStructure(true)
                .Build();
                
                logger.LogInformation("配置 TimeRecord 表");

                fsql.CodeFirst.ConfigEntity<TimeRecord>(t =>
                {
                    t.Property(o => o.Id).IsPrimary(true).IsIdentity(true);
                    t.Property(o => o.Information).StringLength(-1);
                    t.Navigate(o => o.Group, nameof(TimeRecord.GroupId));
                    
                    t.Property(o => o.UpdateTime).ServerTime(DateTimeKind.Utc);
                });

                logger.LogInformation("配置 RecordGroup 表");

                fsql.CodeFirst.ConfigEntity<RecordGroup>(t =>
                {
                    t.Property(o => o.Id).IsPrimary(true).IsIdentity(true);
                    t.Property(o => o.Comment).StringLength(-1);
                    t.Navigate(o => o.Parent, nameof(RecordGroup.ParentId));
                    t.Navigate(o => o.Children, nameof(RecordGroup.ParentId));

                    t.Property(o => o.CreateedTime).ServerTime(DateTimeKind.Utc).CanUpdate(false);
                    t.Property(o => o.EditedTime).ServerTime(DateTimeKind.Utc);
                });

                return fsql;
            })
            .AddSingleton<TimeService>()
            .AddSingleton<TonesFactory>()
            .AddSingleton<AudioService>()
            .AddSingleton<AlarmService>()

            .AddSingleton<TaskbarIconViewModel>()

            .AddSingleton<GroupsViewModel>()
            .AddSingleton<RecordManageViewModel>()
            .AddSingleton<TonesManageViewModel>()
            .AddSingleton<MainViewModel>()

            .AddSingleton<RecordViewModel>()

            .BuildServiceProvider();

            Ioc.Default.ConfigureServices(series);

            var tb = ((TaskbarIcon)this.FindResource("AppTb"));
            tb.ForceCreate();
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }

        private AppSettings GetConfig(ILogger<AppSettings> logger)
        {
            string filePath = "./appsettings.json";

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
