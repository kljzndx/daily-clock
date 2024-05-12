using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public class WindowService : ObservableObject
    {
        private RecordWindow? _wdRecord;

        public bool IsEnabledRecord { get; private set; }

        public void ShowRecord()
        {
            if (_wdRecord != null)
                return;

            _wdRecord = new RecordWindow();
            _wdRecord.Show();

            SetStateProperty(nameof(IsEnabledRecord), true);
        }

        public void CloseRecord()
        {
            if (_wdRecord == null)
                return;

            _wdRecord.Close();
            _wdRecord = null;

            SetStateProperty(nameof(IsEnabledRecord), false);
        }

        private void SetStateProperty(string propName, object value)
        {
            OnPropertyChanging(propName);
            this.GetType().GetProperty(propName)?.SetValue(this, value);
            OnPropertyChanged(propName);
        }
    }
}
