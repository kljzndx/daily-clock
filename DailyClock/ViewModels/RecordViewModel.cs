using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Models.Base;
using DailyClock.Services;
using DailyClock.Services.Tables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class RecordViewModel : BaseObservableRecipient
    {
        private readonly IFreeSql _svcFsql;
        private readonly WindowService _svcWindow;
        private readonly TimeService _svcTime;

        public AppSettings Svc_Settings { get; }
        public RecordTimerService Svc_AlarmTime { get; }
        public RecordGroupService Svc_Group { get; }

        public TimeRecord TheRecord { get; } = new();

        public bool IsEnableCloseTimer { get; private set; }
        public int CloseSecond { get; private set; }

        public RecordViewModel(IFreeSql fsql,
            AppSettings appSettings,
            RecordGroupService groupService,
            RecordTimerService alarmTimeService,
            WindowService windowService, 
            TimeService timeService)
        {
            _svcFsql = fsql;
            _svcWindow = windowService;
            _svcTime = timeService;

            Svc_Settings = appSettings;
            Svc_AlarmTime = alarmTimeService;
            Svc_Group = groupService;

            _svcTime.Tick += SvcTime_Tick;
            _svcWindow.RecordOpened += SvcWindow_RecordOpened;
            _svcWindow.RecordClosed += SvcWindow_RecordClosed;
            TheRecord.PropertyChanged += TheRecord_PropertyChanged;
        }

        [RelayCommand]
        public async Task Submit()
        {
            TheRecord.CreateTime = Svc_AlarmTime.CurrentTime;
            var row = await _svcFsql.Insert(TheRecord).ExecuteAffrowsAsync();
            //logger.LogInformation("已插入：{row} 条数据", row);

            Svc_AlarmTime.IsEnabled = true;
            CloseWindow();
        }

        public void CloseWindow() => _svcWindow.CloseRecord();

        private void SvcWindow_RecordOpened(object? sender, EventArgs e)
        {
            SetStateProperty(nameof(CloseSecond), 30);
            SetStateProperty(nameof(IsEnableCloseTimer), true);
        }

        private void SvcWindow_RecordClosed(object? sender, EventArgs e)
        {
            SetStateProperty(nameof(IsEnableCloseTimer), false);
        }

        private void TheRecord_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetStateProperty(nameof(IsEnableCloseTimer), false);
        }

        private async void SvcTime_Tick(object? sender, DateTime e)
        {
            if (!IsEnableCloseTimer)
                return;

            if (CloseSecond == 0)
            {
                if (!TheRecord.Name.EndsWith("（自动提交）"))
                    TheRecord.Name += "  （自动提交）";

                await Submit();
            }
            else
                SetStateProperty(nameof(CloseSecond), CloseSecond - 1);
        }
    }
}
