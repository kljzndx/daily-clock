using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DailyClock.Services
{
    public partial class TimeService : ObservableObject
    {
        private bool _isInited;
        private DispatcherTimer _timer;

        [ObservableProperty]
        private DateTime _currentTime;

        public event EventHandler<DateTime>? Tick;

        public TimeService()
        {
            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1),
            };

            _timer.Tick += Timer_Tick;
        }

        public void BeginTimer()
        {
            if (_isInited) return;

            _timer.Start();

            _isInited = true;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.UtcNow;
            Tick?.Invoke(this, CurrentTime);
        }
    }
}
