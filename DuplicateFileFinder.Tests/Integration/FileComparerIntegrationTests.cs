using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DuplicateFileFinder;
using DuplicateFileFinder.Data;
using Xunit;

namespace DuplicateFileFinder.Tests.Integration;

public class FileComparerIntegrationTests : IDisposable
{
    private readonly string _rootDir;

    public FileComparerIntegrationTests()
    {
        _rootDir = Directory.CreateTempSubdirectory("DffIntegrationTests_").FullName;
    }

    public void Dispose()
    {
        if (Directory.Exists(_rootDir))
            Directory.Delete(_rootDir, true);
    }

    [Fact]
    public void CompareFiles_AcrossMultipleDirectories_FindsAllDuplicateNames()
    {
        var dir1 = Directory.CreateDirectory(Path.Combine(_rootDir, "dir1"));
        var dir2 = Directory.CreateDirectory(Path.Combine(_rootDir, "dir2"));
        var dir3 = Directory.CreateDirectory(Path.Combine(_rootDir, "dir3"));

        File.WriteAllText(Path.Combine(dir1.FullName, "report.pdf"), "report content 1");
        File.WriteAllText(Path.Combine(dir2.FullName, "report.pdf"), "report content 2");
        File.WriteAllText(Path.Combine(dir3.FullName, "report.pdf"), "report content 3");
        File.WriteAllText(Path.Combine(dir1.FullName, "unique.txt"), "only here");

        var files = GatherFiles(dir1.FullName, dir2.FullName, dir3.FullName);
        var result = FileComparer.CompareFiles(files);

        Assert.Single(result.SameNames);
        Assert.Equal(3, result.SameNames[0].Count);
        Assert.All(result.SameNames[0], fd => Assert.Equal("report.pdf", fd.FileName));
    }

    [Fact]
    public void CompareFiles_AcrossMultipleDirectories_FindsAllDuplicateSizes()
    {
        var dir1 = Directory.CreateDirectory(Path.Combine(_rootDir, "sizeDir1"));
        var dir2 = Directory.CreateDirectory(Path.Combine(_rootDir, "sizeDir2"));

        var content = new byte[512];
        File.WriteAllBytes(Path.Combine(dir1.FullName, "fileA.bin"), content);
        File.WriteAllBytes(Path.Combine(dir2.FullName, "fileB.bin"), content);
        File.WriteAllBytes(Path.Combine(dir1.FullName, "different.bin"), new byte[256]);

        var files = GatherFiles(dir1.FullName, dir2.FullName);
        var result = FileComparer.CompareFiles(files);

        var sameSizeGroup = result.SameSizes.FirstOrDefault(g => g.Count == 2);
        Assert.NotNull(sameSizeGroup);
        Assert.All(sameSizeGroup, fd => Assert.Equal(512, fd.Size));
    }

    [Fact]
    public void CompareFiles_WithNoFiles_ReturnsEmptyResults()
    {
        var result = FileComparer.CompareFiles(new List<FileData>());

        Assert.Empty(result.SameNames);
        Assert.Empty(result.SameSizes);
    }

    [Fact]
    public void CompareFiles_WithAllUniqueFiles_ReturnsNoGroups()
    {
        var dir = Directory.CreateDirectory(Path.Combine(_rootDir, "uniqueDir"));
        File.WriteAllText(Path.Combine(dir.FullName, "alpha.txt"), "a");
        File.WriteAllText(Path.Combine(dir.FullName, "beta.txt"), "bb");
        File.WriteAllText(Path.Combine(dir.FullName, "gamma.txt"), "ccc");

        var files = GatherFiles(dir.FullName);
        var result = FileComparer.CompareFiles(files);

        Assert.Empty(result.SameNames);
        Assert.Empty(result.SameSizes);
    }

    private static List<FileData> GatherFiles(params string[] directories)
    {
        var list = new List<FileData>();
        foreach (var dir in directories)
        {
            list.AddRange(
                Directory.GetFiles(dir)
                         .Select(path => new FileData(new FileInfo(path))));
        }
        return list;
    }
}
