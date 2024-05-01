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
    public partial class RecordViewModel(ILogger<RecordViewModel> logger, IFreeSql fsql, AppSettings appSettings, TimeService timeService, AudioService audioService) : ObservableRecipient
    {
        public AppSettings Svc_Settings { get; } = appSettings;
        public TimeService Svc_Time { get; } = timeService;

        private RecordWindow? _recordWindow;
        private bool _isInited;

        [ObservableProperty]
        private Dictionary<long, string> _groups = [];
        [ObservableProperty]
        private TimeRecord _theRecord = new();

        private bool _isKicked = true;
        private DateTime _kickTime;

        public async Task Init(RecordWindow window)
        {
            _recordWindow = window;

            if (_isInited) return;

            Svc_Time.Tick += Svc_Time_Tick;
            Svc_Time.BeginTimer();

            await LoadGroupData();
            
            _isInited = true;
        }

        public void ShowWindow()
        {
            _isKicked = true;
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
            TheRecord.CreateTime = Svc_Time.CurrentTime;
            var row = await fsql.Insert(TheRecord).ExecuteAffrowsAsync();
            logger.LogInformation("已插入：{row} 条数据", row);

            SetupKickTime();
            _recordWindow?.Close();
        }

        private void SetupKickTime()
        {
            _kickTime = DateTime.UtcNow.AddMinutes(Svc_Settings.KickInterval);
            _isKicked = false;
        }

        private void Svc_Time_Tick(object? sender, DateTime e)
        {
            if (!_isKicked && e > _kickTime)
            {
                _isKicked = true;

                audioService.Play(Svc_Settings.TonesGroups[0]);
                ShowWindow();
            }
        }
    }
}
