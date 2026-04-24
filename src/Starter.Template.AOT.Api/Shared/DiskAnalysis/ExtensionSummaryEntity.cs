namespace Starter.Template.AOT.Api.Shared.DiskAnalysis;

public class ExtensionSummaryEntity
{
    public string Extension { get; set; } = string.Empty;

    public long TotalSize { get; set; }

    public int ItemCount { get; set; }

    public double AverageSize { get; set; }

    public double SizeVariationPercentage { get; set; }
}
