using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services.Tables
{
    public partial class RecordGroup : ObservableObject
    {
        [ObservableProperty]
        private long _id;
        [ObservableProperty]
        private string _name = "";
        [ObservableProperty]
        private string _comment = "";

        [ObservableProperty]
        private string _icon = "";
        [ObservableProperty]
        private string _iconType = "Text";
        [ObservableProperty]
        private string _color = "";

        [ObservableProperty]
        private long _parentId;
        [ObservableProperty]
        private RecordGroup? _parent;
        [ObservableProperty]
        private ObservableCollection<RecordGroup>? _children;

        [ObservableProperty]
        private DateTime _createedTime;
        [ObservableProperty]
        private DateTime _editedTime;
    }
}
