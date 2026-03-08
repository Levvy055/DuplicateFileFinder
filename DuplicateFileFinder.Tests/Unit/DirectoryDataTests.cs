using System.IO;
using DuplicateFileFinder.Data;
using Xunit;

namespace DuplicateFileFinder.Tests.Unit;

public class DirectoryDataTests
{
    [Fact]
    public void DirectoryData_Constructor_SetsDirectoryName()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var di = new DirectoryInfo(dir.FullName);

            var data = new DirectoryData(di);

            Assert.Equal(di.Name, data.DirectoryName);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void DirectoryData_Constructor_SetsFullPath()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var di = new DirectoryInfo(dir.FullName);

            var data = new DirectoryData(di);

            Assert.Equal(di.FullName, data.FullPath);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void DirectoryData_Constructor_SetsFileCount()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            File.WriteAllText(Path.Combine(dir.FullName, "a.txt"), "1");
            File.WriteAllText(Path.Combine(dir.FullName, "b.txt"), "2");
            File.WriteAllText(Path.Combine(dir.FullName, "c.txt"), "3");
            var di = new DirectoryInfo(dir.FullName);

            var data = new DirectoryData(di);

            Assert.Equal(3, data.Files);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void DirectoryData_Constructor_SetsZeroFilesForEmptyDirectory()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var di = new DirectoryInfo(dir.FullName);

            var data = new DirectoryData(di);

            Assert.Equal(0, data.Files);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }
}
