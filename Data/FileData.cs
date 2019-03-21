using System.IO;

namespace DuplicateFileFinder.Data
{
    public class FileData
    {
        private static int _id = 0;

        public FileData(FileInfo fi)
        {
            Id = ++_id;
            FileName = fi.Name;
            FullPath = fi.FullName;
            Size = fi.Length;
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public long Size { get; set; }
        public string FullPath { get; set; }
    }
}
