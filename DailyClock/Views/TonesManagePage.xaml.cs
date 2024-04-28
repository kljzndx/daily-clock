﻿using CommunityToolkit.Mvvm.DependencyInjection;

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
    /// RecordPage.xaml 的交互逻辑
    /// </summary>
    public partial class TonesManagePage : UserControl
    {
        private TonesManageViewModel _viewModel;

        public TonesManagePage()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<TonesManageViewModel>();
            this.DataContext = _viewModel;
        }
    }
}
