using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    public partial class TimeRecord : ObservableObject
    {
        public TimeRecord()
        {
            
        }

        public TimeRecord(string title, string message, DateTime beginTime, DateTime endTime)
        {
            _title = title;
            _message = message;
            _beginTime = beginTime;
            _endTime = endTime;
        }

        [ObservableProperty]
        private long _id;
        [ObservableProperty]
        private string _title = "";
        [ObservableProperty]
        private string _message = "";
        [ObservableProperty]
        private DateTime _beginTime;
        [ObservableProperty]
        private DateTime _endTime;

        [ObservableProperty]
        private DateTime _updateTime;
    }
}
