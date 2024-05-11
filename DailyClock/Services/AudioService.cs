using CommunityToolkit.Mvvm.ComponentModel;

using DailyClock.Models.Tones;

using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public partial class AudioService(TonesFactory tonesFactory) : ObservableObject
    {
        private DirectSoundOut? _dso;
        private Func<TonesFactory, ISampleProvider>? lastFact;

        [ObservableProperty]
        private bool _isLoop;

        public void Play(Func<TonesFactory, ISampleProvider> fact)
        {
            lastFact = fact;
            Play(fact(tonesFactory));
        }

        private void Play(ISampleProvider source)
        {
            if (_dso != null)
                Stop();

            _dso = new();
            _dso.Init(source);
            _dso.Play();
            _dso.PlaybackStopped += Dso_PlaybackStopped;
        }

        public void Stop()
        {
            IsLoop = false;
            _dso?.Stop();
        }

        private void Dso_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (sender is not DirectSoundOut obj)
                return;

            obj.PlaybackStopped -= Dso_PlaybackStopped;
            obj.Dispose();
            _dso = null;

            if (lastFact != null && IsLoop)
                Play(lastFact);
        }
    }
}
