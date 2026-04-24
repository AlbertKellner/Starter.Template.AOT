using System.Text.Json.Serialization;

namespace Starter.Template.AOT.Api.Shared.DiskAnalysis;

public class DiskItemEntity
{
    [JsonInclude]
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonInclude]
    [JsonPropertyName("value")]
    public long Size { get; set; }

    [JsonInclude]
    [JsonPropertyName("color")]
    public string Color { get; set; } = string.Empty;

    [JsonIgnore]
    public string FullPath { get; set; } = string.Empty;

    [JsonIgnore]
    public string Extension { get; set; } = string.Empty;

    [JsonIgnore]
    public bool IsFolder => string.IsNullOrEmpty(Extension);

    [JsonPropertyName("children")]
    [JsonInclude]
    public List<DiskItemEntity>? Children { get; set; }

    [JsonPropertyName("formattedSize")]
    public string FormattedSize
    {
        get
        {
            string[] sizes = ["B", "KB", "MB", "GB", "TB"];
            var len = (double)Size;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }

    public override string ToString() => FullPath;

    public void UpdateFolderSize()
    {
        if (IsFolder && Children is not null)
        {
            Size = Children.Sum(child => child.Size);
        }
    }

    public void SortChildrenBySize()
    {
        if (Children is null)
            return;

        Children.Sort((a, b) => b.Size.CompareTo(a.Size));

        foreach (var child in Children)
        {
            child.SortChildrenBySize();
        }
    }

    public DiskItemEntity? FindFolder(IList<string> pathSegments)
    {
        if (pathSegments is null || pathSegments.Count == 0)
            return this;

        var targetFolderName = pathSegments[0];

        var targetFolder = Children?.FirstOrDefault(child =>
            child.Name.Equals(targetFolderName, StringComparison.OrdinalIgnoreCase) && child.IsFolder);

        if (targetFolder is null)
            return null;

        return targetFolder.FindFolder(pathSegments.Skip(1).ToList());
    }

    public List<ExtensionSummaryEntity> GetTotalSizePerExtension()
    {
        if (Children is null)
            return [];

        return Children
            .GroupBy(file => file.Extension)
            .Select(group =>
            {
                var minSize = group.Min(file => file.Size);
                var maxSize = group.Max(file => file.Size);
                var sizeVariationPercentage = minSize != maxSize
                    ? (double)(maxSize - minSize) / minSize * 100
                    : 0;

                return new ExtensionSummaryEntity
                {
                    Extension = group.Key,
                    TotalSize = group.Sum(file => file.Size),
                    ItemCount = group.Count(),
                    AverageSize = group.Average(file => file.Size),
                    SizeVariationPercentage = sizeVariationPercentage
                };
            })
            .ToList();
    }
}
