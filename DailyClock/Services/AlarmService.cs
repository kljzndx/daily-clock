using CommunityToolkit.Mvvm.ComponentModel;

using DailyClock.Models;
using DailyClock.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public partial class AlarmService : ObservableObject
    {
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

        public AlarmService(AppSettings appSettings, TimeService timeService, AudioService audioService)
        {
            _svcSettings = appSettings;
            _svcTime = timeService;
            _svcAudio = audioService;

            _svcTime.Tick += SvcTime_Tick;
        }

        private void SvcTime_Tick(object? sender, DateTime e)
        {
            CurrentTime = e;

            if (IsEnabled == true)
            {
                if (CountdownSecond < 0)
                {
                    IsHited = false;
                    CountdownSecond = 60 * _svcSettings.HitClockInterval;
                }
                else if (CountdownSecond > 0)
                    CountdownSecond--;
                else
                {
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
