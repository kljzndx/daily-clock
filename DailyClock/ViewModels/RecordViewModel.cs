using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Services;
using DailyClock.Services.Tables;
using DailyClock.Views;

using H.NotifyIcon;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class RecordViewModel(ILogger<RecordViewModel> logger, IFreeSql fsql, AppSettings appSettings, AlarmService alarmTimeService) : ObservableRecipient
    {
        public AppSettings Svc_Settings { get; } = appSettings;
        public AlarmService Svc_AlarmTime { get; } = alarmTimeService;

        private RecordWindow? _recordWindow;
        private bool _isInited;

        [ObservableProperty]
        private Dictionary<long, string> _groups = [];
        [ObservableProperty]
        private TimeRecord _theRecord = new();

        public async Task Init(RecordWindow window)
        {
            _recordWindow = window;

            if (_isInited) return;

            Svc_AlarmTime.Init();

            await LoadGroupData();
            
            _isInited = true;
        }

        public void ShowWindow()
        {
            Svc_AlarmTime.IsEnabled = false;
            new RecordWindow().Show();
        }

        public async Task LoadGroupData()
        {
            logger.LogInformation("开始导入分组数据");
            var groupList = await fsql.Select<RecordGroup>().ToListAsync(t => new ValueTuple<long, long, string>(t.Id, t.ParentId, t.Name));
            logger.LogInformation("已导入 {count} 条数据", groupList.Count);
            Groups = GetTreeList(groupList);
        }

        private Dictionary<long, string> GetTreeList(List<(long id, long pid, string name)> groupList, long searchId = 0, int layer = 0)
        {
            var result = new Dictionary<long, string>();

            var subItems = groupList.Where(t => t.pid == searchId).ToList();
            if (subItems.Count == 0)
                return result;

            string space = "";
            for (int i = 0; i < layer; i++)
            {
                space += "  ";
            }

            foreach (var (id, pid, name) in subItems)
            {
                result[id] = space + name;

                var subs = GetTreeList(groupList, id, layer + 1);

                foreach (var item in subs)
                    result.Add(item.Key, item.Value);
            }

            return result;
        }

        [RelayCommand]
        private async Task Submit()
        {
            TheRecord.CreateTime = Svc_AlarmTime.CurrentTime;
            var row = await fsql.Insert(TheRecord).ExecuteAffrowsAsync();
            logger.LogInformation("已插入：{row} 条数据", row);

            Svc_AlarmTime.IsEnabled = true;
            _recordWindow?.Close();
        }
    }
}
