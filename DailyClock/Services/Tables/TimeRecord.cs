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

        public TimeRecord(string name, string information, DateTime createTime, RecordGroup group)
        {
            _name = name;
            _information = information;
            _createTime = createTime;
            _group = group;
            _groupId = group.Id;
        }

        [ObservableProperty]
        private long _id;
        [ObservableProperty]
        private string _name = "";
        [ObservableProperty]
        private string _information = "";
        [ObservableProperty]
        private long _groupId;
        [ObservableProperty]
        private RecordGroup? _group;

        [ObservableProperty]
        private DateTime _createTime;
        [ObservableProperty]
        private DateTime _updateTime;
    }
}
