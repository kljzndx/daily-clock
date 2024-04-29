using CommunityToolkit.Mvvm.DependencyInjection;

using DailyClock.ViewModels;
using DailyClock.Views;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DailyClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel => (MainViewModel) this.DataContext;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, EventArgs e)
        {
            NavigationBar_ListBox.SelectedIndex = 0;
        }
        
        private void NavigationBar_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationBar_ListBox.Items[NavigationBar_ListBox.SelectedIndex] is ListBoxItem item)
                Main_Frame.Content = item.Tag;
        }
    }
}