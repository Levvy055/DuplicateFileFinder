using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateFileFinder.Data
{
    public class DirectoryData
    {
        private static int _id = 0;

        public DirectoryData(DirectoryInfo fi)
        {
            Id = ++_id;
            DirectoryName = fi.Name;
            FullPath = fi.FullName;
            Files = fi.GetFiles().Length;
        }

        public int Id { get; set; }
        public string DirectoryName { get; set; }
        public long Files { get; set; }
        public string FullPath { get; set; }
    }
}
