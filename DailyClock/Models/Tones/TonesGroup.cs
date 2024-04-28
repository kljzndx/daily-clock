using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Models.Tones
{
    public partial class TonesGroup : ObservableObject
    {
        [ObservableProperty]
        private string _name = "";
        [ObservableProperty]
        private ObservableCollection<ToneProp> _items = [];
    }
}
