﻿using CommunityToolkit.Mvvm.DependencyInjection;

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
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetService<MainViewModel>();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object? sender, EventArgs e)
        {
            Main_Frame.Navigate(new GroupsPage());
        }
    }
}