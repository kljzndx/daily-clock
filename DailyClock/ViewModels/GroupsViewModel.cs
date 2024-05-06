using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Services;
using DailyClock.Services.Tables;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace DailyClock.ViewModels
{
    public partial class GroupsViewModel(RecordGroupService groupService) : ObservableRecipient
    {
        public RecordGroupService SvcGroup { get; } = groupService;

        public async Task InitData()
        {
            await SvcGroup.LoadAsync();
        }
    }
}
