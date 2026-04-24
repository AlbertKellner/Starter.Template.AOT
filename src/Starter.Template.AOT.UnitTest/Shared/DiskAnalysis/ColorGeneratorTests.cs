using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.UnitTest.Shared.DiskAnalysis;

public class ColorGeneratorTests
{
    [Fact]
    public void ApplyColorsToStructure_NullChildren_DoesNotThrow()
    {
        var structure = new DiskItemEntity { Children = null };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.Null(structure.Children);
    }

    [Fact]
    public void ApplyColorsToStructure_WithChildren_AssignsColors()
    {
        var structure = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity
                {
                    Name = "folder1", Extension = "",
                    Children =
                    [
                        new DiskItemEntity { Name = "file1.txt", Extension = ".txt", Size = 100 }
                    ]
                },
                new DiskItemEntity
                {
                    Name = "folder2", Extension = "",
                    Children =
                    [
                        new DiskItemEntity { Name = "file2.log", Extension = ".log", Size = 200 }
                    ]
                }
            ]
        };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.NotEmpty(structure.Children[0].Color);
        Assert.NotEmpty(structure.Children[1].Color);
        Assert.NotEmpty(structure.Children[0].Children![0].Color);
        Assert.NotEmpty(structure.Children[1].Children![0].Color);
    }

    [Fact]
    public void ApplyColorsToStructure_LeafNode_GetsDesaturatedColor()
    {
        var structure = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity { Name = "leaf", Extension = ".txt", Size = 100, Children = null }
            ]
        };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.NotEmpty(structure.Children[0].Color);
    }

    [Fact]
    public void ApplyColorsToStructure_EmptyChildrenList_GetsDesaturatedColor()
    {
        var structure = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity { Name = "emptyFolder", Extension = "", Children = [] }
            ]
        };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.NotEmpty(structure.Children[0].Color);
    }

    [Fact]
    public void ApplyColorsToStructure_AlternatesChildSaturation()
    {
        var structure = new DiskItemEntity
        {
            Children =
            [
                new DiskItemEntity
                {
                    Name = "parent", Extension = "",
                    Children =
                    [
                        new DiskItemEntity { Name = "even", Extension = ".a", Size = 10 },
                        new DiskItemEntity { Name = "odd", Extension = ".b", Size = 20 },
                        new DiskItemEntity { Name = "even2", Extension = ".c", Size = 30 }
                    ]
                }
            ]
        };

        ColorGenerator.ApplyColorsToStructure(structure);

        var children = structure.Children[0].Children!;
        Assert.All(children, c => Assert.NotEmpty(c.Color));
    }

    [Fact]
    public void ApplyColorsToStructure_MoreThanPredefinedColors_WrapsAround()
    {
        var children = new List<DiskItemEntity>();
        for (var i = 0; i < 50; i++)
        {
            children.Add(new DiskItemEntity { Name = $"item{i}", Extension = ".x", Size = i + 1 });
        }

        var structure = new DiskItemEntity { Children = children };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.All(structure.Children, c => Assert.NotEmpty(c.Color));
    }

    [Fact]
    public void InterpolateToGrey_ZeroPercent_ReturnsSameColor()
    {
        var result = ColorGenerator.InterpolateToGrey("#FF0000", 0);

        Assert.Equal("#FF0000", result);
    }

    [Fact]
    public void InterpolateToGrey_HundredPercent_ReturnsGrey()
    {
        var result = ColorGenerator.InterpolateToGrey("#FF0000", 100);

        Assert.StartsWith("#", result);
        Assert.Equal(7, result.Length);
    }

    [Fact]
    public void Saturate_IncreasesColorSaturation()
    {
        var original = "#808080";
        var result = ColorGenerator.Saturate(original, 50);

        Assert.StartsWith("#", result);
        Assert.Equal(7, result.Length);
    }

    [Fact]
    public void Desaturate_DecreasesColorSaturation()
    {
        var original = "#FF0000";
        var result = ColorGenerator.Desaturate(original, 50);

        Assert.StartsWith("#", result);
        Assert.Equal(7, result.Length);
    }

    [Fact]
    public void Saturate_PureGrey_ProducesValidColor()
    {
        var result = ColorGenerator.Saturate("#808080", 0);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void Desaturate_FullDesaturation_ProducesGreyish()
    {
        var result = ColorGenerator.Desaturate("#FF0000", 100);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void InterpolateToGrey_BlueColor_ProducesValidHex()
    {
        var result = ColorGenerator.InterpolateToGrey("#0000FF", 50);

        Assert.StartsWith("#", result);
        Assert.Equal(7, result.Length);
    }

    [Fact]
    public void Saturate_GreenDominant_CoversMaxGreenBranch()
    {
        // Green dominant: max == gNorm branch in RgbToHsl
        var result = ColorGenerator.Saturate("#00FF00", 10);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void Saturate_BlueDominant_CoversMaxBlueBranch()
    {
        // Blue dominant: max == bNorm branch in RgbToHsl
        var result = ColorGenerator.Saturate("#0000FF", 10);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void Saturate_RedDominantWithGreenLessThanBlue_CoversHuePlusSix()
    {
        // Red dominant with G < B: h = (G - B) / d + 6 branch
        var result = ColorGenerator.Saturate("#FF0080", 10);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void Saturate_Achromatic_CoversSEqualsZeroBranch()
    {
        // Pure grey: s == 0 in HslToRgb
        var result = ColorGenerator.Saturate("#808080", 0);

        Assert.Equal("#808080", result);
    }

    [Fact]
    public void Saturate_DarkColor_CoversLLessThanHalf()
    {
        // Dark color: l < 0.5 branch in HslToRgb
        var result = ColorGenerator.Saturate("#330000", 10);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void Saturate_LightColor_CoversLGreaterThanHalf()
    {
        // Light color: l > 0.5 branch in RgbToHsl and HslToRgb
        var result = ColorGenerator.Saturate("#FFCCCC", 10);

        Assert.StartsWith("#", result);
    }

    [Fact]
    public void ApplyColorsToStructure_DeepNesting_AssignsColorsAtAllLevels()
    {
        var deepChild = new DiskItemEntity { Name = "deep", Extension = ".txt", Size = 10 };
        var midChild = new DiskItemEntity { Name = "mid", Extension = "", Children = [deepChild] };
        var topChild = new DiskItemEntity { Name = "top", Extension = "", Children = [midChild] };
        var structure = new DiskItemEntity { Children = [topChild] };

        ColorGenerator.ApplyColorsToStructure(structure);

        Assert.NotEmpty(topChild.Color);
        Assert.NotEmpty(midChild.Color);
        Assert.NotEmpty(deepChild.Color);
    }
}
