using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class MainViewModel(ILogger<MainViewModel> logger, AppSettings conf) : ObservableRecipient
    {
        [ObservableProperty]
        private string _mes = "Nomal";

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
    }
}
