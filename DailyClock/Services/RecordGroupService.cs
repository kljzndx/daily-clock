using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Services.Tables;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public partial class RecordGroupService : ObservableObject
    {
        private IFreeSql _fsql;

        public ObservableCollection<RecordGroup> Items { get; } = [];
        public ObservableCollection<KeyValuePair<long, string>> StrTree { get; } = [];

        [ObservableProperty]
        private RecordGroup? _selected;

        public RecordGroupService(IFreeSql fsql)
        {
            _fsql = fsql;
        }

        public async Task LoadAsync()
        {
            var isAny = _fsql.Select<RecordGroup>().Any();

            if (!isAny)
            {
                _ = await _fsql.Insert(new List<RecordGroup>{
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

            var list = await _fsql.Select<RecordGroup>().ToTreeListAsync();
            var strTree = GetStringTreeList(list);

            Items.Clear();
            foreach (var item in list)
                Items.Add(item);

            StrTree.Clear();
            foreach (var item in strTree)
                StrTree.Add(item);
        }

        [RelayCommand]
        private async Task CreateRootGroup()
        {
            var ng = new RecordGroup();

            ng.Id = await _fsql.Insert(ng).ExecuteIdentityAsync();

            Items.Add(ng);
            StrTree.Add(new(ng.Id, ng.Name));
        }

        private bool CanCreateSubGroup => Selected != null && Selected.ParentId == 0;

        [RelayCommand(CanExecute = nameof(CanCreateSubGroup))]
        private async Task CreateSubGroup()
        {
            if (!(Selected != null && Selected.ParentId == 0))
                return;

            var ng = new RecordGroup(Selected);

            ng.Id = await _fsql.Insert(ng).ExecuteIdentityAsync();

            Selected.Children.Add(ng);

            int tid = (int)StrTree.First(o => o.Key == Selected.Id).Key + Selected.Children.Count;
            StrTree.Insert(tid, new(ng.Id, "  " + ng.Name));
        }

        private List<KeyValuePair<long, string>> GetStringTreeList(IEnumerable<RecordGroup> groupList, int layer = 0)
        {
            var result = new List<KeyValuePair<long, string>>();

            string space = "";
            for (int i = 0; i < layer; i++)
            {
                space += "  ";
            }

            foreach (var group in groupList)
            {
                result.Add(new KeyValuePair<long, string>(group.Id, space + group.Name));

                var subs = GetStringTreeList(group.Children, layer + 1);
                result.AddRange(subs);
            }

            return result;
        }


        partial void OnSelectedChanged(RecordGroup? oldValue, RecordGroup? newValue)
        {
            if (oldValue is not null)
                oldValue.PropertyChanged -= SelectedGroup_PropertyChanged;

            if (newValue is not null)
            {
                newValue.PropertyChanged += SelectedGroup_PropertyChanged;

                var Status = $"已选择 {newValue.Id}  {newValue.Name}";
                //logger.LogInformation(Status);
            }
        }

        private async void SelectedGroup_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not RecordGroup gp || e.PropertyName is null)
            {
                //logger.LogWarning("找不到事件触发者");
                return;
            }

            PropertyInfo propInfo = gp.GetType().GetProperty(e.PropertyName)!;
            await _fsql.Update<RecordGroup>(gp.Id).SetDto(new Dictionary<string, object> { [e.PropertyName] = propInfo.GetValue(gp)! }).ExecuteAffrowsAsync();

            if (e.PropertyName == nameof(RecordGroup.Name))
            {
                int tid = StrTree.IndexOf(StrTree.First(o => o.Key == gp.Id));
                var space = gp.Parent == null ? "" : "  ";

                StrTree.RemoveAt(tid);
                StrTree.Insert(tid, new(gp.Id, space + gp.Name));
            }

            var status = $"Group[{gp.Id}].{e.PropertyName} 已设置为 {propInfo.GetValue(gp)}";
            //logger.LogInformation(status);
        }
    }
}
