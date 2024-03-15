using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyClock.Models
{
    public partial class AppSettings : ObservableObject
    {
        [ObservableProperty]
        private string _test1 = "Hello world";

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, options: new() { WriteIndented = true });
        }
    }
}
