using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Services.Tables;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class MainViewModel(ILogger<MainViewModel> logger, AppSettings conf, IFreeSql fsql) : ObservableRecipient
    {
        [ObservableProperty]
        private string _mes = "Nomal";
        [ObservableProperty]
        private string _timeMes = "";

        [ObservableProperty]
        private ObservableCollection<TimeRecord> _data = new();

        [RelayCommand]
        private void Hello()
        {
            logger.LogInformation("Hello World");
            Mes = "Logged";
        }

        [RelayCommand]
        private void TestConf()
        {
            logger.LogInformation("Testing read conf");
            Mes = conf.Test1;
        }

        [RelayCommand]
        private void TestReadDb()
        {
            Data.Clear();
            foreach (var item in fsql.Select<TimeRecord>().ToList(a => a))
            {
                Data.Add(item);
            }
        }

        [RelayCommand]
        private async Task TestWriteDb()
        {
            DateTime now = DateTime.Now;
            DateTime before = now - TimeSpan.FromMinutes(5);

            TimeRecord record = new(TimeMes, TimeMes, before, now);

            record.Id = await fsql.Insert<TimeRecord>().AppendData(record).ExecuteIdentityAsync();
            Data.Add(record);

        }
    }
}
