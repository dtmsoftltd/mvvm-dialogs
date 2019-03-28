using System.ComponentModel;
using System.Windows;
using MvvmDialogs;

namespace Demo.ActivateNonModalDialog
{
    public static class Extensions
    {
        public static bool? Activate(this IDialogService self, INotifyPropertyChanged viewModel)
        {
            foreach (var window in Application.Current.Windows)
            {
                if (((Window)window).DataContext == viewModel)
                {
                    return ((Window)window).Activate();
                }
            }

            return null;
        }
    }
}
