using CommunityToolkit.Mvvm.ComponentModel;

using DailyClock.Models;
using DailyClock.Views;

using Serilog;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public partial class AlarmService : ObservableObject
    {
        private const string LOG_HEAD = "[定时记录器]";

        private ILogger _svcLogger;
        private AppSettings _svcSettings;
        private TimeService _svcTime;
        private AudioService _svcAudio;

        [ObservableProperty]
        private bool? _isEnabled = false;
        [ObservableProperty]
        private bool _isHited;
        [ObservableProperty]
        private DateTime _currentTime;
        [ObservableProperty]
        private DateTime _hitTime;
        [ObservableProperty]
        private int _countdownSecond = -1;

        public event EventHandler<DateTime>? HitStarted;

        public AlarmService(ILogger logger, AppSettings appSettings, TimeService timeService, AudioService audioService)
        {
            _svcLogger = logger;
            _svcSettings = appSettings;
            _svcTime = timeService;
            _svcAudio = audioService;

            _svcTime.Tick += SvcTime_Tick;
        }

        partial void OnIsEnabledChanged(bool? value)
        {
            string status = value == true ? "已开启" : value == null ? "已暂停" : "已停止";
            _svcLogger.Information("定时器状态： {stu}", status);
        }

        private void SvcTime_Tick(object? sender, DateTime e)
        {
            CurrentTime = e;

            if (IsEnabled == true)
            {
                if (CountdownSecond < 0)
                {
                    _svcLogger.Information("{head}  初始化定时器", LOG_HEAD);
                    IsHited = false;
                    CountdownSecond = 60 * _svcSettings.HitClockInterval;
                }
                else if (CountdownSecond > 0)
                {
                    CountdownSecond--;
                    _svcLogger.Information("{head}  当前计数： {cd}", LOG_HEAD, CountdownSecond);
                }
                else
                {
                    _svcLogger.Information("{head}  时间已到，开始弹窗", LOG_HEAD);
                    HitClock();
                    new RecordWindow().Show();
                }
            }
            else if (IsEnabled == null)
                HitTime = e.AddSeconds(CountdownSecond);
            else
            {
                CountdownSecond = -1;
                HitTime = e.AddMinutes(_svcSettings.HitClockInterval);
            }
        }

        private void HitClock()
        {
            CountdownSecond = -1;
            IsHited = true;
            IsEnabled = false;

            _svcAudio.Play(fa => fa.CreateTone(_svcSettings.TonesGroups[0]));
            HitStarted?.Invoke(this, HitTime);
        }
    }
}
