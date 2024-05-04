using CommunityToolkit.Mvvm.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel Main => Ioc.Default.GetRequiredService<MainViewModel>();
        public RecordManageViewModel RecordManage => Ioc.Default.GetRequiredService<RecordManageViewModel>();
        public GroupsViewModel Groups => Ioc.Default.GetRequiredService<GroupsViewModel>();
        public TonesManageViewModel TonesManage => Ioc.Default.GetRequiredService<TonesManageViewModel>();

        public RecordViewModel Record => Ioc.Default.GetRequiredService<RecordViewModel>();
        public TaskbarIconViewModel TaskbarIcon => Ioc.Default.GetRequiredService<TaskbarIconViewModel>();
    }
}
