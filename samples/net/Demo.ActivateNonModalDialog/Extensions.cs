using System.ComponentModel;
using System.Windows;
using MvvmDialogs;

namespace Demo.ActivateNonModalDialog
{
    public static class Extensions
    {
        public static bool? Activate(this IDialogService self, INotifyPropertyChanged viewModel)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == viewModel)
                {
                    return window.Activate();
                }
            }

            return null;
        }
    }
}
