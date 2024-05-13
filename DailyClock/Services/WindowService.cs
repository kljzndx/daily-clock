using CommunityToolkit.Mvvm.Input;

using DailyClock.Models.Base;
using DailyClock.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Services
{
    public class WindowService : BaseObservableObject
    {
        private RecordWindow? _wdRecord;

        public bool IsEnabledRecord { get; private set; }

        public event EventHandler? RecordOpened;
        public event EventHandler? RecordClosed;

        public void ShowRecord()
        {
            if (_wdRecord != null)
                return;

            _wdRecord = new RecordWindow();
            _wdRecord.Show();

            SetStateProperty(nameof(IsEnabledRecord), true);
            RecordOpened?.Invoke(this, EventArgs.Empty);
        }

        public void CloseRecord()
        {
            if (_wdRecord == null)
                return;

            _wdRecord.Close();
            _wdRecord = null;

            SetStateProperty(nameof(IsEnabledRecord), false);
            RecordClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
