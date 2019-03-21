using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuplicateFileFinder.Data;

namespace DuplicateFileFinder
{
    public class Results
    {
        public Dictionary<string, List<FileData>> SameNames { get; } = new Dictionary<string, List<FileData>>();
        public Dictionary<long, List<FileData>> SameSizes { get; } = new Dictionary<long, List<FileData>>();

    }
}
