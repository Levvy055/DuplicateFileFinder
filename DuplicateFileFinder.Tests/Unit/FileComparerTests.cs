using System.Collections.Generic;
using System.IO;
using DuplicateFileFinder;
using DuplicateFileFinder.Data;
using Xunit;

namespace DuplicateFileFinder.Tests.Unit;

public class FileComparerTests
{
    [Fact]
    public void CompareFiles_WithNullInput_ReturnsEmptyResults()
    {
        var result = FileComparer.CompareFiles(null);

        Assert.Empty(result.SameNames);
        Assert.Empty(result.SameSizes);
    }

    [Fact]
    public void CompareFiles_WithEmptyList_ReturnsEmptyResults()
    {
        var result = FileComparer.CompareFiles(new List<FileData>());

        Assert.Empty(result.SameNames);
        Assert.Empty(result.SameSizes);
    }

    [Fact]
    public void CompareFiles_WithUniqueFiles_ReturnsEmptyResults()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var file1 = Path.Combine(dir.FullName, "alpha.txt");
            var file2 = Path.Combine(dir.FullName, "beta.txt");
            File.WriteAllText(file1, "hello");
            File.WriteAllText(file2, "hello world");

            var files = new List<FileData>
            {
                new FileData(new FileInfo(file1)),
                new FileData(new FileInfo(file2)),
            };

            var result = FileComparer.CompareFiles(files);

            Assert.Empty(result.SameNames);
            Assert.Empty(result.SameSizes);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void CompareFiles_WithDuplicateFileNames_GroupsMatchingFiles()
    {
        var dir1 = Directory.CreateTempSubdirectory();
        var dir2 = Directory.CreateTempSubdirectory();
        try
        {
            var file1 = Path.Combine(dir1.FullName, "duplicate.txt");
            var file2 = Path.Combine(dir2.FullName, "duplicate.txt");
            File.WriteAllText(file1, "content a");
            File.WriteAllText(file2, "content b");

            var files = new List<FileData>
            {
                new FileData(new FileInfo(file1)),
                new FileData(new FileInfo(file2)),
            };

            var result = FileComparer.CompareFiles(files);

            Assert.Single(result.SameNames);
            Assert.Equal(2, result.SameNames[0].Count);
        }
        finally
        {
            Directory.Delete(dir1.FullName, true);
            Directory.Delete(dir2.FullName, true);
        }
    }

    [Fact]
    public void CompareFiles_WithDuplicateFileSizes_GroupsMatchingFiles()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var file1 = Path.Combine(dir.FullName, "first.txt");
            var file2 = Path.Combine(dir.FullName, "second.txt");
            File.WriteAllText(file1, "abc");
            File.WriteAllText(file2, "xyz");

            var files = new List<FileData>
            {
                new FileData(new FileInfo(file1)),
                new FileData(new FileInfo(file2)),
            };

            var result = FileComparer.CompareFiles(files);

            Assert.Single(result.SameSizes);
            Assert.Equal(2, result.SameSizes[0].Count);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }

    [Fact]
    public void CompareFiles_WithNoNameDuplicates_SameNamesIsEmpty()
    {
        var dir = Directory.CreateTempSubdirectory();
        try
        {
            var file1 = Path.Combine(dir.FullName, "one.txt");
            var file2 = Path.Combine(dir.FullName, "two.txt");
            File.WriteAllText(file1, "same");
            File.WriteAllText(file2, "same");

            var files = new List<FileData>
            {
                new FileData(new FileInfo(file1)),
                new FileData(new FileInfo(file2)),
            };

            var result = FileComparer.CompareFiles(files);

            Assert.Empty(result.SameNames);
        }
        finally
        {
            Directory.Delete(dir.FullName, true);
        }
    }
}
