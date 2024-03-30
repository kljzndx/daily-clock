using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public TimeRecord(string title, string message, DateTime beginTime, DateTime endTime, RecordGroup group)
        {
            _title = title;
            _message = message;
            _beginTime = beginTime;
            _endTime = endTime;
            _group = group;
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
        private long _groupId;
        [ObservableProperty]
        private RecordGroup? _group;

        [ObservableProperty]
        private DateTime _updateTime;
    }
}
