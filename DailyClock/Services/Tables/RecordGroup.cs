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
        public RecordGroup()
        {
            
        }

        public RecordGroup(RecordGroup parent) : this()
        {
            _parent = parent;
            _parentId = parent.Id;
        }

        [ObservableProperty]
        private long _id;
        [ObservableProperty]
        private string _name = "新分组";
        [ObservableProperty]
        private string _comment = "";

        [ObservableProperty]
        private string _icon = "☺️";
        [ObservableProperty]
        private string _iconType = "Text";
        [ObservableProperty]
        private string _color = "#FF000000";

        [ObservableProperty]
        private long _parentId;
        [ObservableProperty]
        private RecordGroup? _parent;
        [ObservableProperty]
        private ObservableCollection<RecordGroup> _children = new();

        [ObservableProperty]
        private DateTime _createedTime;
        [ObservableProperty]
        private DateTime _editedTime;
    }
}
