namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

public class DiskItemsGetAllOutput
{
    public string Name { get; set; } = string.Empty;
    public long Value { get; set; }
    public string Color { get; set; } = string.Empty;
    public string FormattedSize { get; set; } = string.Empty;
    public List<DiskItemsGetAllOutput>? Children { get; set; }
}
