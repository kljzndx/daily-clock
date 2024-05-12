﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Services;
using DailyClock.Services.Tables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class RecordViewModel(IFreeSql fsql, AppSettings appSettings, RecordGroupService groupService, RecordTimerService alarmTimeService, WindowService windowService) : ObservableRecipient
    {
        private WindowService _svcWindow { get; } = windowService;

        public AppSettings Svc_Settings { get; } = appSettings;
        public RecordTimerService Svc_AlarmTime { get; } = alarmTimeService;
        public RecordGroupService Svc_Group { get; } = groupService;

        [ObservableProperty]
        private TimeRecord _theRecord = new();

        public async Task Submit()
        {
            TheRecord.CreateTime = Svc_AlarmTime.CurrentTime;
            var row = await fsql.Insert(TheRecord).ExecuteAffrowsAsync();
            //logger.LogInformation("已插入：{row} 条数据", row);

            Svc_AlarmTime.IsEnabled = true;
            CloseWindow();
        }

        public void CloseWindow() => _svcWindow.CloseRecord();
    }
}
