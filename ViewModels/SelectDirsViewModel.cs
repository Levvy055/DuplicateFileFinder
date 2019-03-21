using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DuplicateFileFinder.Annotations;
using DuplicateFileFinder.Data;
using DuplicateFileFinder.Views;
using Ookii.Dialogs.Wpf;

namespace DuplicateFileFinder.ViewModels
{
    public class SelectDirsViewModel : INotifyPropertyChanged
    {
        private readonly DelegateCommand _commandAdd;
        private readonly DelegateCommand _commandFind;
        private readonly DelegateCommand _commandClear;

        public SelectDirsViewModel()
        {
            _commandAdd = new DelegateCommand(OnAddExecute, CanExecuteAdd);
            _commandFind = new DelegateCommand(OnFindExecute, CanExecuteFind);
            _commandClear = new DelegateCommand(OnClearExecute, CanExecuteClear);

            Directories.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(Directories));
            };
        }

        private bool CanExecuteAdd(object arg)
        {
            return true;
        }

        private void OnAddExecute(object obj)
        {
            var dialog = new VistaFolderBrowserDialog();
            if (dialog.ShowDialog(Application.Current.MainWindow).GetValueOrDefault())
            {
                var path = dialog.SelectedPath;
                if (!Directories.Where((dd, i) => dd.FullPath.Equals(path)).Any())
                {
                    var dd = new DirectoryData(new DirectoryInfo(path));
                    Directories.Add(dd);
                }
                else
                {
                    MessageBox.Show("Direcotry already added!",
                        "Warning", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        private bool CanExecuteFind(object arg)
        {
            return Directories.Count > 0;
        }

        private void OnFindExecute(object obj)
        {
            var mw = Application.Current.MainWindow as MainWindow;
            mw?.GoTo(mw.ShowResultsWindow);
        }

        private bool CanExecuteClear(object arg)
        {
            return Directories.Count > 0;
        }

        private void OnClearExecute(object obj)
        {
            Directories.Clear();
        }

        public ObservableCollection<DirectoryData> Directories { get; } = new ObservableCollection<DirectoryData>();
        public ICommand CommandAdd => _commandAdd;
        public ICommand CommandFind => _commandFind;
        public ICommand CommandClear => _commandClear;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) { return false; }
            field = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            _commandAdd.InvokeCanExecuteChanged();
            _commandFind.InvokeCanExecuteChanged();
            _commandClear.InvokeCanExecuteChanged();
        }
    }
}
