using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DailyClock.Models;
using DailyClock.Models.Base;
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
    public partial class TonesManageViewModel : BaseObservableRecipient
    {
        public AppSettings Settings { get; }
        private readonly AudioService _audioService;

        public TonesManageViewModel(AudioService audioService, AppSettings settings)
        {
            Settings = settings;
            _audioService = audioService;

            WatchSubObject(this, _audioService, nameof(AudioService.IsPlaying), (p, s) =>
            {
                p.StopCommand.NotifyCanExecuteChanged();
                p.PlayGroupCommand.NotifyCanExecuteChanged();
                p.PlayToneCommand.NotifyCanExecuteChanged();
                p.LoopPlayGroupCommand.NotifyCanExecuteChanged();
            });
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoopPlayGroupCommand))]
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
        private bool CanRemoveProp => CanReadProp && SelectedGroup!.Items.Count > 1;
        private bool CanStop => _audioService.IsPlaying;
        private bool CanPlayGroup => !CanStop && CanReadGroup && SelectedGroup!.Items.Count > 0;
        private bool CanPlayProp => !CanStop && CanReadProp;

        [RelayCommand(CanExecute = nameof(CanPlayProp))]
        private void PlayTone()
        {
            _audioService.Play(fa => fa.CreateTone(SelectedProp!));
        }

        [RelayCommand(CanExecute = nameof(CanPlayGroup))]
        private void PlayGroup()
        {
            _audioService.Play(fa => fa.CreateTone(SelectedGroup!));
        }

        [RelayCommand(CanExecute = nameof(CanPlayGroup))]
        private void LoopPlayGroup()
        {
            _audioService.Play(fa => fa.CreateTone(SelectedGroup!));
            _audioService.IsLoop = true;
        }

        [RelayCommand(CanExecute = nameof(CanStop))]
        private void Stop() => _audioService.Stop();

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

        [RelayCommand(CanExecute = nameof(CanRemoveProp))]
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
