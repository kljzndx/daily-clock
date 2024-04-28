using DailyClock.Models.Tones;

using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public class AudioService(TonesFactory tonesFactory)
    {
        private DirectSoundOut? _dso;

        public void Play(TonesGroup source)
        {
            if (_dso != null)
                _dso.Stop();

            var isp = tonesFactory.CreateTone(source);

            _dso = new();
            _dso.Init(isp);
            _dso.Play();
            _dso.PlaybackStopped += Dso_PlaybackStopped;
        }

        public void Play(ToneProp source)
        {
            if (_dso != null)
                _dso.Stop();

            var isp = tonesFactory.CreateTone(source);

            _dso = new();
            _dso.Init(isp);
            _dso.Play();
            _dso.PlaybackStopped += Dso_PlaybackStopped;
        }

        public void Stop()
        {
            _dso?.Stop();
        }

        private void Dso_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (sender is not DirectSoundOut obj)
                return;

            obj.PlaybackStopped -= Dso_PlaybackStopped;
            obj.Dispose();
        }
    }
}
