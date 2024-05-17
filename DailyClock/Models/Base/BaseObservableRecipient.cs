using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        protected void WatchSubObject<TParent, TSub>(TParent parent, TSub sub, string propName, Action<TParent, TSub> callback) where TParent : class where TSub : INotifyPropertyChanged
        {
            sub.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propName)
                    callback(parent, sub);
            };
        }
    }
}
