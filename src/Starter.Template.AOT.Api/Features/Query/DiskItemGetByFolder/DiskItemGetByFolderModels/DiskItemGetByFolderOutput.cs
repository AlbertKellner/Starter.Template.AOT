namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public class DiskItemGetByFolderOutput
{
    public string DriveId { get; init; } = string.Empty;
    public string FolderPath { get; init; } = string.Empty;
    public DiskItemFolderOutput? Folder { get; init; }
}

public class DiskItemFolderOutput
{
    public string Name { get; init; } = string.Empty;
    public long SizeBytes { get; init; }
    public string FormattedSize { get; init; } = string.Empty;
    public bool IsFolder { get; init; }
    public string Extension { get; init; } = string.Empty;
    public List<DiskItemFolderOutput> Children { get; init; } = [];
}
