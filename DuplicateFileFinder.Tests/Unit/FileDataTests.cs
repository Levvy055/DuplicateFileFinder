using System.IO;
using DuplicateFileFinder.Data;
using Xunit;

namespace DuplicateFileFinder.Tests.Unit;

public class FileDataTests
{
    [Fact]
    public void FileData_Constructor_SetsFileName()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(dir.FullName, "test.txt");
            File.WriteAllText(path, "data");
            var fi = new FileInfo(path);

            var fileData = new FileData(fi);

            Assert.Equal("test.txt", fileData.FileName);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void FileData_Constructor_SetsFullPath()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(dir.FullName, "test.txt");
            File.WriteAllText(path, "data");
            var fi = new FileInfo(path);

            var fileData = new FileData(fi);

            Assert.Equal(fi.FullName, fileData.FullPath);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void FileData_Constructor_SetsSize()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var path = Path.Combine(dir.FullName, "test.txt");
            File.WriteAllBytes(path, new byte[] { 1, 2, 3, 4, 5 });
            var fi = new FileInfo(path);

            var fileData = new FileData(fi);

            Assert.Equal(5, fileData.Size);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void FileData_Constructor_AssignsIncrementingId()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var path1 = Path.Combine(dir.FullName, "file1.txt");
            var path2 = Path.Combine(dir.FullName, "file2.txt");
            File.WriteAllText(path1, "a");
            File.WriteAllText(path2, "b");

            var fd1 = new FileData(new FileInfo(path1));
            var fd2 = new FileData(new FileInfo(path2));

            Assert.Equal(fd1.Id + 1, fd2.Id);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }
}
