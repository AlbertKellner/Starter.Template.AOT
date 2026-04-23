namespace Starter.Template.AOT.Api.Shared.DiskItem;

public class DiskItemEntity
{
    public string Name { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Color { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public bool IsFolder => string.IsNullOrEmpty(Extension);
    public List<DiskItemEntity> Children { get; set; } = [];

    public string FormattedSize
    {
        get
        {
            string[] sizes = ["B", "KB", "MB", "GB", "TB"];
            double len = Size;
            var order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }

    public void UpdateFolderSize()
    {
        if (IsFolder)
            Size = Children.Sum(child => child.Size);
    }

    public void SortChildrenBySize()
    {
        Children.Sort((a, b) => b.Size.CompareTo(a.Size));

        foreach (var child in Children)
            child.SortChildrenBySize();
    }

    public DiskItemEntity? FindFolder(IList<string> pathSegments)
    {
        if (pathSegments == null || pathSegments.Count == 0)
            return this;

        var targetName = pathSegments[0];
        var targetFolder = Children.FirstOrDefault(c =>
            c.Name.Equals(targetName, StringComparison.OrdinalIgnoreCase) && c.IsFolder);

        return targetFolder?.FindFolder(pathSegments.Skip(1).ToList());
    }
}
