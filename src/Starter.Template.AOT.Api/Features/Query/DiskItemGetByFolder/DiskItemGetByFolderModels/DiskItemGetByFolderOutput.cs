namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public class DiskItemGetByFolderOutput
{
    public string Name { get; set; } = string.Empty;
    public long Value { get; set; }
    public string FormattedSize { get; set; } = string.Empty;
    public List<DiskItemGetByFolderOutput>? Children { get; set; }
}
