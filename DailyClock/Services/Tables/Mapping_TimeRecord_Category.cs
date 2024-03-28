using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    public partial class Mapping_TimeRecord_Tag : ObservableObject
    {
        [ObservableProperty]
        private long _id;

        [ObservableProperty]
        private long _recordId;
        [ObservableProperty]
        private TimeRecord? _record;

        [ObservableProperty]
        private long _tagId;
        [ObservableProperty]
        private RecordTag? _tag;
    }
}
