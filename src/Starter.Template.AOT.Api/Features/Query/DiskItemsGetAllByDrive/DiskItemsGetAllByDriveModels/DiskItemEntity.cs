namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAllByDrive;

public class DiskItemEntity
{
    public string Name { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public bool IsFolder { get; set; }
    public string Extension { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public List<DiskItemEntity> Children { get; set; } = [];

    public void UpdateFolderSize()
    {
        if (!IsFolder)
            return;

        foreach (var child in Children)
        {
            child.UpdateFolderSize();
        }

        SizeBytes = Children.Sum(c => c.SizeBytes);
    }

    public void SortChildrenBySize()
    {
        Children.Sort((a, b) => b.SizeBytes.CompareTo(a.SizeBytes));

        foreach (var child in Children)
        {
            child.SortChildrenBySize();
        }
    }
}
