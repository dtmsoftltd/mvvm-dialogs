﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using MvvmDialogs.DialogTypeLocators;
using MvvmDialogs.FrameworkDialogs;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using MvvmDialogs.Properties;
using DialogResult = System.Windows.Forms.DialogResult;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace MvvmDialogs
{
    /// <summary>
    /// Class abstracting the interaction between view models and views when it comes to
    /// opening dialogs using the MVVM pattern in WPF.
    /// </summary>
    public class DialogService : IDialogService
    {
        private readonly IDialogTypeLocator dialogTypeLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="dialogTypeLocator">
        /// The dialog type locator. Specifying a <see cref="IDialogTypeLocator"/> is required when
        /// using <see cref="IDialogService.ShowDialog"/>.
        /// </param>
        public DialogService(IDialogTypeLocator dialogTypeLocator = null)
        {
            this.dialogTypeLocator = dialogTypeLocator;
        }
                
        #region IDialogService Members

        /// <summary>
        /// Displays a non-modal dialog of specified type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="viewModel">The view model of the new dialog.</param>
        /// <typeparam name="T">The type of the dialog to show.</typeparam>
        public void Show<T>(INotifyPropertyChanged ownerViewModel, INotifyPropertyChanged viewModel) where T : Window
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            Show(ownerViewModel, viewModel, typeof(T));
        }

        /// <summary>
        /// Displays a non-modal dialog of a type that is determined by the
        /// <see cref="IDialogTypeLocator"/> specified in
        /// <see cref="DialogService(IDialogTypeLocator)"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="viewModel">The view model of the new dialog.</param>
        public void Show(INotifyPropertyChanged ownerViewModel, INotifyPropertyChanged viewModel)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");
            if (dialogTypeLocator == null)
                throw new InvalidOperationException(Resources.ImplicitUseProhibited);

            Type dialogType = dialogTypeLocator.LocateDialogTypeFor(viewModel);
            Show(ownerViewModel, viewModel, dialogType);
        }

        /// <summary>
        /// Displays a modal dialog of specified type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="viewModel">The view model of the new dialog.</param>
        /// <typeparam name="T">The type of the dialog to show.</typeparam>
        /// <returns>
        /// A nullable value of type <see cref="bool"/> that signifies how a window was closed by
        /// the user.
        /// </returns>
        public bool? ShowDialog<T>(
            INotifyPropertyChanged ownerViewModel,
            INotifyPropertyChanged viewModel)
            where T : Window
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");

            return ShowDialog(ownerViewModel, viewModel, typeof(T));
        }

        /// <summary>
        /// Displays a modal dialog of a type that is determined by the
        /// <see cref="IDialogTypeLocator" /> specified in
        /// <see cref="DialogService(IDialogTypeLocator)" />.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="viewModel">The view model of the new dialog.</param>
        /// <returns>
        /// A nullable value of type <see cref="bool" /> that signifies how a window was closed by
        /// the user.
        /// </returns>
        public bool? ShowDialog(INotifyPropertyChanged ownerViewModel, INotifyPropertyChanged viewModel)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (viewModel == null)
                throw new ArgumentNullException("viewModel");
            if (dialogTypeLocator == null)
                throw new InvalidOperationException(Resources.ImplicitUseProhibited);

            Type dialogType = dialogTypeLocator.LocateDialogTypeFor(viewModel);
            return ShowDialog(ownerViewModel, viewModel, dialogType);
        }

        /// <summary>
        /// Displays a message box that has a message and that returns a result.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="messageBoxText">
        /// A <see cref="string"/> that specifies the text to display.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            INotifyPropertyChanged ownerViewModel,
            string messageBoxText)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");

            return MessageBox.Show(FindOwnerWindow(ownerViewModel), messageBoxText);
        }

        /// <summary>
        /// Displays a message box that has a message and title bar caption; and that returns a
        /// result.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="messageBoxText">
        /// A <see cref="string"/> that specifies the text to display.
        /// </param>
        /// <param name="caption">
        /// A <see cref="string"/> that specifies the title bar caption to display.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            INotifyPropertyChanged ownerViewModel,
            string messageBoxText,
            string caption)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");

            return MessageBox.Show(FindOwnerWindow(ownerViewModel), messageBoxText, caption);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and button; and that
        /// returns a result.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="messageBoxText">
        /// A <see cref="string"/> that specifies the text to display.
        /// </param>
        /// <param name="caption">
        /// A <see cref="string"/> that specifies the title bar caption to display.
        /// </param>
        /// <param name="button">
        /// A <see cref="MessageBoxButton"/> value that specifies which button or buttons to
        /// display.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            INotifyPropertyChanged ownerViewModel,
            string messageBoxText,
            string caption,
            MessageBoxButton button)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");

            return MessageBox.Show(
                FindOwnerWindow(ownerViewModel),
                messageBoxText,
                caption,
                button);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and
        /// that returns a result.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="messageBoxText">
        /// A <see cref="string"/> that specifies the text to display.
        /// </param>
        /// <param name="caption">
        /// A <see cref="string"/> that specifies the title bar caption to display.
        /// </param>
        /// <param name="button">
        /// A <see cref="MessageBoxButton"/> value that specifies which button or buttons to
        /// display.
        /// </param>
        /// <param name="icon">
        /// A <see cref="MessageBoxImage"/> value that specifies the icon to display.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            INotifyPropertyChanged ownerViewModel,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            
            return MessageBox.Show(
                FindOwnerWindow(ownerViewModel),
                messageBoxText,
                caption,
                button,
                icon);
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and
        /// that accepts a default message box result and returns a result.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="messageBoxText">
        /// A <see cref="string"/> that specifies the text to display.
        /// </param>
        /// <param name="caption">
        /// A <see cref="string"/> that specifies the title bar caption to display.
        /// </param>
        /// <param name="button">
        /// A <see cref="MessageBoxButton"/> value that specifies which button or buttons to
        /// display.
        /// </param>
        /// <param name="icon">
        /// A <see cref="MessageBoxImage"/> value that specifies the icon to display.
        /// </param>
        /// <param name="defaultResult">
        /// A <see cref="MessageBoxResult"/> value that specifies the default result of the
        /// message box.
        /// </param>
        /// <returns>
        /// A <see cref="MessageBoxResult"/> value that specifies which message box button is
        /// clicked by the user.
        /// </returns>
        public MessageBoxResult ShowMessageBox(
            INotifyPropertyChanged ownerViewModel,
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");

            return MessageBox.Show(
                FindOwnerWindow(ownerViewModel),
                messageBoxText,
                caption,
                button,
                icon,
                defaultResult);
        }

        /// <summary>
        /// Shows the <see cref="OpenFileDialog"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="settings">The settings for the open file dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        public bool? ShowOpenFileDialog(
            INotifyPropertyChanged ownerViewModel,
            OpenFileDialogSettings settings)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (settings == null)
                throw new ArgumentNullException("settings");

            var dialog = new OpenFileDialogWrapper(settings);
            return dialog.ShowDialog(FindOwnerWindow(ownerViewModel));
        }

        /// <summary>
        /// Shows the <see cref="SaveFileDialog"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="settings">The settings for the save file dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        public bool? ShowSaveFileDialog(
            INotifyPropertyChanged ownerViewModel,
            SaveFileDialogSettings settings)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (settings == null)
                throw new ArgumentNullException("settings");

            var dialog = new SaveFileDialogWrapper(settings);
            return dialog.ShowDialog(FindOwnerWindow(ownerViewModel));
        }

        /// <summary>
        /// Shows the <see cref="FolderBrowserDialog"/>.
        /// </summary>
        /// <param name="ownerViewModel">
        /// A view model that represents the owner window of the dialog.
        /// </param>
        /// <param name="settings">The settings for the folder browser dialog.</param>
        /// <returns>
        /// If the user clicks the OK button of the dialog that is displayed, true is returned;
        /// otherwise false.
        /// </returns>
        public bool? ShowFolderBrowserDialog(
            INotifyPropertyChanged ownerViewModel,
            FolderBrowserDialogSettings settings)
        {
            if (ownerViewModel == null)
                throw new ArgumentNullException("ownerViewModel");
            if (settings == null)
                throw new ArgumentNullException("settings");

            using (var dialog = new FolderBrowserDialogWrapper(settings))
            {
                DialogResult result = dialog.ShowDialog(new WindowWrapper(FindOwnerWindow(ownerViewModel)));
                return result == DialogResult.OK;
            }
        }

        #endregion

        private static void Show(
            INotifyPropertyChanged ownerViewModel,
            INotifyPropertyChanged viewModel,
            Type dialogType)
        {
            Window dialog = CreateDialog(dialogType, ownerViewModel, viewModel);
            dialog.Show();
        }

        private static bool? ShowDialog(
            INotifyPropertyChanged ownerViewModel,
            INotifyPropertyChanged viewModel,
            Type dialogType)
        {
            Window dialog = CreateDialog(dialogType, ownerViewModel, viewModel);
            return dialog.ShowDialog();
        }

        private static Window CreateDialog(
            Type dialogType,
            INotifyPropertyChanged ownerViewModel,
            INotifyPropertyChanged viewModel)
        {
            var dialog = (Window)Activator.CreateInstance(dialogType);
            dialog.Owner = FindOwnerWindow(ownerViewModel);
            dialog.DataContext = viewModel;

            return dialog;
        }

        /// <summary>
        /// Finds window corresponding to specified view model.
        /// </summary>
        private static Window FindOwnerWindow(INotifyPropertyChanged viewModel)
        {
            FrameworkElement view = DialogServiceBehaviors.Views.SingleOrDefault(
                registeredView => ReferenceEquals(registeredView.DataContext, viewModel));
            
            if (view == null)
                throw new ArgumentException(Resources.ViewModelNotReferenced.CurrentFormat(viewModel.GetType()));

            // Get owner window
            Window owner = view.GetOwner();
            if (owner == null)
                throw new InvalidOperationException(Resources.ViewNotRegistered.CurrentFormat(view.GetType()));

            return owner;
        }
    }
}