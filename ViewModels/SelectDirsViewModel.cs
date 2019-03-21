﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
        private bool _findingRunning;
        private readonly BackgroundWorker _finder = new BackgroundWorker();
        private readonly BackgroundWorker _analyzer = new BackgroundWorker();
        private int _progressF;
        private int _progressA;
        private int _progressFmax = 100;
        private int _progressAmax = 100;

        public SelectDirsViewModel()
        {
            _commandAdd = new DelegateCommand(OnAddExecute);
            _commandFind = new DelegateCommand(OnFindExecute, CanExecuteFind);
            _commandClear = new DelegateCommand(OnClearExecute, CanExecuteClear);

            Directories.CollectionChanged += (sender, args) =>
            {
                OnPropertyChanged(nameof(Directories));
            };

            _finder.DoWork += Find;
            _analyzer.DoWork += Analyze;

            _finder.RunWorkerCompleted += (sender, args) =>
            {
                _analyzer.RunWorkerAsync(args.Result);
            };
            _analyzer.RunWorkerCompleted += (sender, args) =>
            {
                _findingRunning = false;
                if (args.Cancelled) { return; }
                var mw = Application.Current.MainWindow as MainWindow;
                // mw.ShowResultsWindow.ApplyResults(); TODO: create
                mw?.GoTo(mw.ShowResultsWindow);
            };

            _finder.ProgressChanged += (sender, args) => ProgressOfFinder = args.ProgressPercentage;
            _analyzer.ProgressChanged += (sender, args) => ProgressOfAnalyzer = args.ProgressPercentage;
            _finder.WorkerReportsProgress = true;
            _analyzer.WorkerReportsProgress = true;

            
        }

        private void Analyze(object sender, DoWorkEventArgs e)
        {
            var files = e.Argument as List<FileData>;

            e.Cancel = false;
        }

        private void Find(object sender, DoWorkEventArgs e)
        {
            var list = Directories.Select(dd => dd.FullPath).ToList();
            var allFiles = new List<FileData>();
            var i = 0;
            foreach (var dir in list)
            {
                allFiles.AddRange(Directory.GetFiles(dir).Select(filePath =>
                {
                    _finder.ReportProgress(i++);
                    Thread.Sleep(500);
                    return new FileData(new FileInfo(filePath));
                }));
            }

            e.Cancel = false;
            e.Result = allFiles;
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
            FindingIsRunning = true;
            _progressAmax = (int)Directories.Select(dd => dd.Files).Sum();
            _progressFmax = _progressAmax;
            _finder.RunWorkerAsync();
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

        public bool FindingIsRunning
        {
            get => _findingRunning;
            set => SetProperty(ref _findingRunning, value);
        }

        public int ProgressOfFinder
        {
            get => _progressF;
            set => SetProperty(ref _progressF, value);
        }

        public int ProgressFinderMax
        {
            get => _progressFmax;
            set => SetProperty(ref _progressFmax, value);
        }

        public int ProgressAnalyzerMax
        {
            get => _progressAmax;
            set => SetProperty(ref _progressAmax, value);
        }

        public int ProgressOfAnalyzer
        {
            get => _progressA;
            set => SetProperty(ref _progressA, value);
        }

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
