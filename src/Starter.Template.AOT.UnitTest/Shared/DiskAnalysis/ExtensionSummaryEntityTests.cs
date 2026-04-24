using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.UnitTest.Shared.DiskAnalysis;

public class ExtensionSummaryEntityTests
{
    [Fact]
    public void Properties_SetAndGet_WorkCorrectly()
    {
        var entity = new ExtensionSummaryEntity
        {
            Extension = ".txt",
            TotalSize = 1024,
            ItemCount = 5,
            AverageSize = 204.8,
            SizeVariationPercentage = 50.0
        };

        Assert.Equal(".txt", entity.Extension);
        Assert.Equal(1024, entity.TotalSize);
        Assert.Equal(5, entity.ItemCount);
        Assert.Equal(204.8, entity.AverageSize);
        Assert.Equal(50.0, entity.SizeVariationPercentage);
    }

    [Fact]
    public void DefaultValues_AreCorrect()
    {
        var entity = new ExtensionSummaryEntity();

        Assert.Equal(string.Empty, entity.Extension);
        Assert.Equal(0, entity.TotalSize);
        Assert.Equal(0, entity.ItemCount);
        Assert.Equal(0, entity.AverageSize);
        Assert.Equal(0, entity.SizeVariationPercentage);
    }
}
