using CommunityToolkit.Mvvm.DependencyInjection;

using DailyClock.Services;
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
using System.Windows.Shapes;

namespace DailyClock.Views
{
    /// <summary>
    /// RecordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RecordWindow : Window
    {
        private RecordViewModel _viewModel => (RecordViewModel) this.DataContext;

        public RecordWindow()
        {
            InitializeComponent();
        }

        private void ManageGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
        }

        private async void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.Submit();
            _viewModel.CloseWindow();
        }

        private void Close_CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.CloseWindow();
        }
    }
}
