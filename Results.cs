using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicateFileFinder.Data;

namespace DuplicateFileFinder
{
    public class Results
    {
        public ObservableCollection<ObservableCollection<FileData>> SameNames { get; } = new ObservableCollection<ObservableCollection<FileData>>();
        public ObservableCollection<ObservableCollection<FileData>> SameSizes { get; } = new ObservableCollection<ObservableCollection<FileData>>();

    }
}
