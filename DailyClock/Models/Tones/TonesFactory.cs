using CommunityToolkit.Mvvm.ComponentModel;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Models.Tones
{
    public class TonesFactory
    {
        public ISampleProvider CreateTone(ToneProp prop)
        {
            var basic = new SignalGenerator { Frequency = prop.Freq > 20 ? prop.Freq : 20, Gain = prop.Gain };
            if (prop.Duration_seconds > 0.001)
                return basic.Take(TimeSpan.FromSeconds(prop.Duration_seconds));
            else return basic;
        }

        public ISampleProvider CreateTone(TonesGroup group)
        {
            if (group == null || group.Items.Count == 0)
                throw new ArgumentNullException(nameof(group));

            ISampleProvider result = CreateTone(new ToneProp(20, 0, 0.002, 0));

            foreach (var item in group.Items)
                result = result.FollowedBy(TimeSpan.FromSeconds(item.Delay_seconds), CreateTone(item));

            return result;
        }
    }
}
