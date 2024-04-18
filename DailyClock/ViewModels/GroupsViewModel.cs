using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class GroupsViewModel(ILogger<MainViewModel> logger, IFreeSql fsql) : ObservableRecipient
    {
        [ObservableProperty]
        private ObservableCollection<RecordGroup> _groups = new();
        [ObservableProperty]
        private RecordGroup? _selected;
        [ObservableProperty]
        private string _status = "";

        public async Task InitData()
        {
            bool isAny = await fsql.Select<RecordGroup>().AnyAsync();

            if (!isAny)
            {
                logger.LogInformation("添加初始分组数据");

                _ = await fsql.Insert(new List<RecordGroup>{
                    new()
                    {
                        Id = 1,
                        Name = "工作",
                        Icon = "⚒️",
                        Color = "#FFFFD700"
                    },
                    new()
                    {
                        Id = 2,
                        Name = "娱乐",
                        Icon = "🎮",
                        Color = "#FF00BFFF"
                    },
                }).ExecuteAffrowsAsync();
            }

            await LoadGroups();
        }

        [RelayCommand]
        private async Task LoadGroups()
        {
            logger.LogInformation("读取分组数据");
            Groups = new(await fsql.Select<RecordGroup>().ToTreeListAsync());
        }

        [RelayCommand]
        private async Task CreateGroup()
        {
            var ng = new RecordGroup()
            {
                Parent = Selected,
                ParentId = Selected?.Id ?? 0
            };

            ng.Id = await fsql.Insert(ng).ExecuteIdentityAsync();

            if (Selected == null) Groups.Add(ng);
            else Selected.Children.Add(ng);
        }

        partial void OnSelectedChanged(RecordGroup? oldValue, RecordGroup? newValue)
        {
            if (oldValue is not null)
                oldValue.PropertyChanged -= SelectedGroup_PropertyChanged;

            if (newValue is not null)
            {
                newValue.PropertyChanged += SelectedGroup_PropertyChanged;

                Status = $"已选择 {newValue.Id}  {newValue.Name}";
                logger.LogInformation(Status);
            }
        }

        private async void SelectedGroup_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not RecordGroup gp || e.PropertyName is null)
            {
                logger.LogWarning("找不到事件触发者");
                return;
            }

            PropertyInfo propInfo = gp.GetType().GetProperty(e.PropertyName)!;

            await fsql.Update<RecordGroup>(gp.Id).SetDto(new Dictionary<string, object> { [e.PropertyName] = propInfo.GetValue(gp)! }).ExecuteAffrowsAsync();

            Status = $"Group[{gp.Id}].{e.PropertyName} 已设置为 {propInfo.GetValue(gp)}";
            logger.LogInformation(Status);
        }
    }
}
