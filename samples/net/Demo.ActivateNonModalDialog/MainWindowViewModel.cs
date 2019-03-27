using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmDialogs;

namespace Demo.NonModalDialog
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly DispatcherTimer timer;

        private INotifyPropertyChanged dialogViewModel;

        public MainWindowViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (sender, e) => ImplicitShowCommand.Execute(null);
            timer.Start();

            ImplicitShowCommand = new RelayCommand(ImplicitShow);
        }

        public ICommand ImplicitShowCommand { get; }

        private void ImplicitShow()
        {
            if (dialogViewModel == null)
            {
                dialogViewModel = new CurrentTimeDialogViewModel();
                dialogService.Show(this, dialogViewModel);
            }
            else
            {
                dialogService.Activate(dialogViewModel);
            }
        }
    }
}
