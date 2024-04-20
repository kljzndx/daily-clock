using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class RecordViewModel(ILogger<RecordViewModel> logger, IFreeSql fsql) : ObservableRecipient
    {

    }
}
