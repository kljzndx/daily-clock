using CommunityToolkit.Mvvm.DependencyInjection;

using DailyClock.Services.Tables;
using DailyClock.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyClock.Views
{
    /// <summary>
    /// GroupsPage.xaml 的交互逻辑
    /// </summary>
    public partial class GroupsPage : Page
    {
        private GroupsViewModel _viewModel;

        public GroupsPage()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<GroupsViewModel>();
            this.DataContext = _viewModel;

            this.Loaded += GroupsPage_Loaded;
        }

        private async void GroupsPage_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.InitData();
        }

        private void Group_TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _viewModel.Selected = e.NewValue as RecordGroup;
        }
    }
}
