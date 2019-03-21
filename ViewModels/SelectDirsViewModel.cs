using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

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
            
        }

        private bool CanExecuteAdd(object arg)
        {
            return true;
        }

        private void OnAddExecute(object obj)
        {
            
        }

        private bool CanExecuteFind(object arg)
        {
            return Directories != null && Directories.Length > 0;
        }

        private void OnFindExecute(object obj)
        {
            var files = Directory.GetFiles("C:\\Users\\lluka\\Documents\\temp\\png");

            foreach (var f in files)
            {
                var fi = new FileInfo(f);
                var lvi = new ListViewItem();
                //DirectoriesListView.Items.Add(lvi);
            }
        }

        private bool CanExecuteClear(object arg)
        {
            return CanExecuteFind(arg);
        }

        private void OnClearExecute(object obj)
        {
            
        }

        public string[] Directories { get; private set; }
        public ICommand CommandAdd => _commandAdd;
        public ICommand CommandFind => _commandFind;
        public ICommand CommandClear => _commandClear;

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) { return false; }
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}
