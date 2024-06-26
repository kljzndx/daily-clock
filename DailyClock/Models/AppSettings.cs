﻿using CommunityToolkit.Mvvm.ComponentModel;

using DailyClock.Models.Tones;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows;

namespace DailyClock.Models
{
    public partial class AppSettings : ObservableObject
    {
        [ObservableProperty]
        private string _fileVersion = "1.0";

        [ObservableProperty]
        private ObservableCollection<TonesGroup> _tonesGroups = [];

        [ObservableProperty]
        private double _recWindowX = SystemParameters.WorkArea.Width - 200;
        [ObservableProperty]
        private double _recWindowY = SystemParameters.WorkArea.Height - 300;
        [ObservableProperty]
        private int _recordGroupSelectedId = 0;
        [ObservableProperty]
        private int _hitClockInterval = 5;

        public void Save(){
            OnPropertyChanged("Manual Save");
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, options: new() { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) });
        }
    }
}
