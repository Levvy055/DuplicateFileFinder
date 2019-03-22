using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicateFileFinder.Data;

namespace DuplicateFileFinder
{
    public class FileComparer
    {
        public static Results CompareFiles(List<FileData> files)
        {
            var res = new Results();
            var sameNames = files?.GroupBy(fd => fd.FileName).Where(group=>group.Count()>1);
            if (sameNames != null)
            {
                foreach (var sameName in sameNames)
                {
                    res.SameNames.Add(new ObservableCollection<FileData>(sameName.ToList()));
                }
            }
            else
            {
                Debug.WriteLine("No same names");
            }
            var sameSizes = files?.GroupBy(fd => fd.Size).Where(group => group.Count() > 1);
            if (sameSizes != null)
            {
                foreach (var sameSize in sameSizes)
                {
                    res.SameSizes.Add(new ObservableCollection<FileData>(sameSize.ToList()));
                }
            }
            else
            {
                Debug.WriteLine("No same sizes");
            }

            return res;
        }

        public static int CompareImages()
        {
            return 0;
        }
    }
}
