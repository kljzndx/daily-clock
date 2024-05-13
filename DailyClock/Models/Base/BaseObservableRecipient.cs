using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.Models.Base
{
    public class BaseObservableRecipient : ObservableRecipient
    {
        protected void SetStateProperty(string propName, object value)
        {
            OnPropertyChanging(propName);
            this.GetType().GetProperty(propName)?.SetValue(this, value);
            OnPropertyChanged(propName);
        }
    }
}
