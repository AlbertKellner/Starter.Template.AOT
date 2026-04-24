using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.UnitTest.Shared.DiskAnalysis;

public class DiskItemEntityTests
{
    [Fact]
    public void IsFolder_WithEmptyExtension_ReturnsTrue()
    {
        var item = new DiskItemEntity { Extension = "" };

        Assert.True(item.IsFolder);
    }

    [Fact]
    public void IsFolder_WithExtension_ReturnsFalse()
    {
        var item = new DiskItemEntity { Extension = ".txt" };

        Assert.False(item.IsFolder);
    }

    [Fact]
    public void FormattedSize_Bytes_ReturnsCorrectFormat()
    {
        var item = new DiskItemEntity { Size = 500 };

        Assert.Equal("500 B", item.FormattedSize);
    }

    [Fact]
    public void FormattedSize_Kilobytes_ReturnsCorrectFormat()
    {
        var item = new DiskItemEntity { Size = 2048 };

        Assert.Equal("2 KB", item.FormattedSize);
    }

    [Fact]
    public void FormattedSize_Megabytes_ReturnsCorrectFormat()
    {
        var item = new DiskItemEntity { Size = 1048576 };

        Assert.Equal("1 MB", item.FormattedSize);
    }

    [Fact]
    public void FormattedSize_Gigabytes_ReturnsCorrectFormat()
    {
        var item = new DiskItemEntity { Size = 1073741824 };

        Assert.Equal("1 GB", item.FormattedSize);
    }

    [Fact]
    public void FormattedSize_Terabytes_ReturnsCorrectFormat()
    {
        var item = new DiskItemEntity { Size = 1099511627776 };

        Assert.Equal("1 TB", item.FormattedSize);
    }

    [Fact]
    public void FormattedSize_Zero_ReturnsZeroBytes()
    {
        var item = new DiskItemEntity { Size = 0 };

        Assert.Equal("0 B", item.FormattedSize);
    }

    [Fact]
    public void ToString_ReturnsFullPath()
    {
        var item = new DiskItemEntity { FullPath = "C:/test/file.txt" };

        Assert.Equal("C:/test/file.txt", item.ToString());
    }

    [Fact]
    public void UpdateFolderSize_WithChildren_SumsChildSizes()
    {
        var folder = new DiskItemEntity
        {
            Extension = "",
            Children =
            [
                new DiskItemEntity { Size = 100, Extension = ".txt" },
                new DiskItemEntity { Size = 200, Extension = ".log" }
            ]
        };

        folder.UpdateFolderSize();

        Assert.Equal(300, folder.Size);
    }

    [Fact]
    public void UpdateFolderSize_FileWithExtension_DoesNotUpdate()
    {
        var file = new DiskItemEntity { Size = 50, Extension = ".txt", Children = null };

        file.UpdateFolderSize();

        Assert.Equal(50, file.Size);
    }

    [Fact]
    public void UpdateFolderSize_FolderWithNullChildren_DoesNotThrow()
    {
        var folder = new DiskItemEntity { Extension = "", Children = null };

        folder.UpdateFolderSize();

        Assert.Equal(0, folder.Size);
    }

    [Fact]
    public void SortChildrenBySize_SortsDescending()
    {
        var folder = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity { Name = "small", Size = 10 },
                new DiskItemEntity { Name = "large", Size = 1000 },
                new DiskItemEntity { Name = "medium", Size = 500 }
            ]
        };

        folder.SortChildrenBySize();

        Assert.Equal("large", folder.Children[0].Name);
        Assert.Equal("medium", folder.Children[1].Name);
        Assert.Equal("small", folder.Children[2].Name);
    }

    [Fact]
    public void SortChildrenBySize_NullChildren_DoesNotThrow()
    {
        var folder = new DiskItemEntity { Children = null };

        folder.SortChildrenBySize();

        Assert.Null(folder.Children);
    }

    [Fact]
    public void SortChildrenBySize_RecursivelySortsNestedChildren()
    {
        var folder = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity
                {
                    Name = "parent", Size = 300,
                    Children =
                    [
                        new DiskItemEntity { Name = "childSmall", Size = 100 },
                        new DiskItemEntity { Name = "childLarge", Size = 200 }
                    ]
                }
            ]
        };

        folder.SortChildrenBySize();

        Assert.Equal("childLarge", folder.Children[0].Children![0].Name);
        Assert.Equal("childSmall", folder.Children[0].Children![1].Name);
    }

    [Fact]
    public void FindFolder_NullSegments_ReturnsSelf()
    {
        var item = new DiskItemEntity { Name = "root" };

        var result = item.FindFolder(null!);

        Assert.Same(item, result);
    }

    [Fact]
    public void FindFolder_EmptySegments_ReturnsSelf()
    {
        var item = new DiskItemEntity { Name = "root" };

        var result = item.FindFolder([]);

        Assert.Same(item, result);
    }

    [Fact]
    public void FindFolder_ExistingFolder_ReturnsFolder()
    {
        var target = new DiskItemEntity { Name = "SubFolder", Extension = "", Children = [] };
        var root = new DiskItemEntity
        {
            Name = "root",
            Children = [target]
        };

        var result = root.FindFolder(["SubFolder"]);

        Assert.Same(target, result);
    }

    [Fact]
    public void FindFolder_NestedFolder_ReturnsDeepFolder()
    {
        var deep = new DiskItemEntity { Name = "Deep", Extension = "", Children = [] };
        var mid = new DiskItemEntity { Name = "Mid", Extension = "", Children = [deep] };
        var root = new DiskItemEntity { Name = "root", Children = [mid] };

        var result = root.FindFolder(["Mid", "Deep"]);

        Assert.Same(deep, result);
    }

    [Fact]
    public void FindFolder_NonExistentFolder_ReturnsNull()
    {
        var root = new DiskItemEntity { Name = "root", Children = [] };

        var result = root.FindFolder(["NonExistent"]);

        Assert.Null(result);
    }

    [Fact]
    public void FindFolder_CaseInsensitive_FindsFolder()
    {
        var target = new DiskItemEntity { Name = "MyFolder", Extension = "", Children = [] };
        var root = new DiskItemEntity { Name = "root", Children = [target] };

        var result = root.FindFolder(["myfolder"]);

        Assert.Same(target, result);
    }

    [Fact]
    public void FindFolder_SkipsFiles_OnlyMatchesFolders()
    {
        var file = new DiskItemEntity { Name = "Target", Extension = ".txt" };
        var root = new DiskItemEntity { Name = "root", Children = [file] };

        var result = root.FindFolder(["Target"]);

        Assert.Null(result);
    }

    [Fact]
    public void GetTotalSizePerExtension_NullChildren_ReturnsEmpty()
    {
        var item = new DiskItemEntity { Children = null };

        var result = item.GetTotalSizePerExtension();

        Assert.Empty(result);
    }

    [Fact]
    public void GetTotalSizePerExtension_WithFiles_ReturnsGroupedSummary()
    {
        var item = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity { Extension = ".txt", Size = 100 },
                new DiskItemEntity { Extension = ".txt", Size = 200 },
                new DiskItemEntity { Extension = ".log", Size = 500 }
            ]
        };

        var result = item.GetTotalSizePerExtension();

        Assert.Equal(2, result.Count);

        var txtSummary = result.First(r => r.Extension == ".txt");
        Assert.Equal(300, txtSummary.TotalSize);
        Assert.Equal(2, txtSummary.ItemCount);
        Assert.Equal(150, txtSummary.AverageSize);
        Assert.True(txtSummary.SizeVariationPercentage > 0);

        var logSummary = result.First(r => r.Extension == ".log");
        Assert.Equal(500, logSummary.TotalSize);
        Assert.Equal(1, logSummary.ItemCount);
        Assert.Equal(0, logSummary.SizeVariationPercentage);
    }

    [Fact]
    public void GetTotalSizePerExtension_SameSize_ZeroVariation()
    {
        var item = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity { Extension = ".dat", Size = 100 },
                new DiskItemEntity { Extension = ".dat", Size = 100 }
            ]
        };

        var result = item.GetTotalSizePerExtension();
        var summary = result.Single();

        Assert.Equal(0, summary.SizeVariationPercentage);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var item = new DiskItemEntity();

        Assert.Equal(string.Empty, item.Name);
        Assert.Equal(0, item.Size);
        Assert.Equal(string.Empty, item.Color);
        Assert.Equal(string.Empty, item.FullPath);
        Assert.Equal(string.Empty, item.Extension);
        Assert.Null(item.Children);
    }
}
