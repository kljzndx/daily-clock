using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Models.Tones;
using DailyClock.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyClock.ViewModels
{
    public partial class TonesManageViewModel(AudioService audioService, AppSettings settings) : ObservableRecipient
    {
        public AppSettings Settings => settings;
        private AudioService _audioService => audioService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(PlayGroupCommand))]
        [NotifyCanExecuteChangedFor(nameof(PlayToneCommand))]

        [NotifyCanExecuteChangedFor(nameof(RemoveGroupCommand))]

        [NotifyCanExecuteChangedFor(nameof(CreatePropCommand))]
        [NotifyCanExecuteChangedFor(nameof(RemovePropCommand))]
        private TonesGroup? _selectedGroup;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemovePropCommand))]
        [NotifyCanExecuteChangedFor(nameof(PlayToneCommand))]
        private ToneProp? _selectedProp;

        partial void OnSelectedGroupChanged(TonesGroup? oldValue, TonesGroup? newValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= Selected_PropertyChanged;
            if (newValue != null)
                newValue.PropertyChanged += Selected_PropertyChanged;
        }

        partial void OnSelectedPropChanged(ToneProp? oldValue, ToneProp? newValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= Selected_PropertyChanged;
            if (newValue != null)
                newValue.PropertyChanged += Selected_PropertyChanged;
        }

        private bool CanReadGroup => SelectedGroup != null;
        private bool CanReadProp => SelectedProp != null && CanReadGroup;
        private bool CanRemoveGroup => CanReadGroup && Settings.TonesGroups.Count > 1;

        [RelayCommand(CanExecute = nameof(CanReadProp))]
        private void PlayTone()
        {
            _audioService.Play(SelectedProp!);
        }

        [RelayCommand(CanExecute = nameof(CanReadGroup))]
        private void PlayGroup()
        {
            _audioService.Play(SelectedGroup!);
        }

        [RelayCommand]
        private void CreateGroup()
        {
            Settings.TonesGroups.Add(new());
            Settings.Save();
        }

        [RelayCommand(CanExecute = nameof(CanRemoveGroup))]
        private void RemoveGroup()
        {
            Settings.TonesGroups.Remove(SelectedGroup!);
            Settings.Save();
        }


        [RelayCommand(CanExecute = nameof(CanReadGroup))]
        private void CreateProp()
        {
            SelectedGroup!.Items.Add(new());
            Settings.Save();
        }

        [RelayCommand(CanExecute = nameof(CanReadProp))]
        private void RemoveProp()
        {
            SelectedGroup!.Items.Remove(SelectedProp!);
            Settings.Save();
        }

        private void Selected_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Settings.Save();
        }
    }
}
