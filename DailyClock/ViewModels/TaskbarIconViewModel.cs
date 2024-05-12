using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Services;
using DailyClock.ViewModels.ValueConverters;
using DailyClock.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class TaskbarIconViewModel : ObservableRecipient
    {
        private TimeService _svcTime;
        private RecordTimerService _svcAlarm;
        private WindowService _svcWindow;

        [ObservableProperty]
        private string _message = "服务初始化中...";

        public TaskbarIconViewModel(TimeService timeService, RecordTimerService alarmService, WindowService windowService)
        {
            _svcTime = timeService;
            _svcAlarm = alarmService;
            _svcWindow = windowService;
            
            _svcTime.Tick += (s, e) => Compute();
        }

        [RelayCommand]
        private void ShowRecWindow()
        {
            _svcWindow.ShowRecord();
        }

        [RelayCommand]
        private void Exit()
        {
            App.Current.Shutdown();
        }

        private void Compute()
        {
            string curTime = $"当前时间： {_svcAlarm.CurrentTime.ToZhString()}";
            string enabled = $"定时记录器： {(_svcAlarm.IsEnabled == true ? "已开启" : _svcAlarm.IsEnabled == null ? "已暂停" : "未开启")}";
            string hitTime = $"提醒时间： {_svcAlarm.HitTime.ToZhString()}{Environment.NewLine}离提醒还剩： {_svcAlarm.CountdownSecond}秒";

            Message = curTime + Environment.NewLine + enabled;

            if (_svcAlarm.IsEnabled != false)
                Message += Environment.NewLine + hitTime;
        }
    }
}
