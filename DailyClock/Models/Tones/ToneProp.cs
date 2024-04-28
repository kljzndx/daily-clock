using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Models.Tones
{
    public partial class ToneProp : ObservableObject
    {
        public ToneProp()
        {
            
        }

        public ToneProp(double freq, double gain, double duration_seconds, double delay_seconds)
        {
            _freq = freq;
            _gain = gain;
            _duration_seconds = duration_seconds;
            _delay_seconds = delay_seconds;
        }

        [ObservableProperty]
        private double _freq;
        [ObservableProperty]
        private double _gain;
        [ObservableProperty]
        private double _duration_seconds;
        [ObservableProperty]
        private double _delay_seconds;
    }
}
