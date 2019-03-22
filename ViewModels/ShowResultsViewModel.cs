using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DuplicateFileFinder.Annotations;
using DuplicateFileFinder.Data;
using DuplicateFileFinder.Views;

namespace DuplicateFileFinder.ViewModels
{
    public class ShowResultsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ObservableCollection<FileData>> _sameNames;
        private ObservableCollection<ObservableCollection<FileData>> _sameSizes;
        private readonly DelegateCommand _commandBack;

        public ShowResultsViewModel()
        {
            _commandBack = new DelegateCommand(OnBackExecute);
        }

        public void ApplyResults(Results results)
        {
            SameNames = results.SameNames;
            SameSizes = results.SameSizes;
        }

        private void OnBackExecute(object obj)
        {
            var mw = Application.Current.MainWindow as MainWindow;
            mw?.GoTo(mw.SelectDirsWindow);
        }

        public ObservableCollection<ObservableCollection<FileData>> SameNames
        {
            get => _sameNames;
            set => SetProperty(ref _sameNames, value);
        }

        public ObservableCollection<ObservableCollection<FileData>> SameSizes
        {
            get => _sameSizes;
            set => SetProperty(ref _sameSizes, value);
        }

        public ICommand CommandAdd => _commandBack;

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
        }
    }
}
