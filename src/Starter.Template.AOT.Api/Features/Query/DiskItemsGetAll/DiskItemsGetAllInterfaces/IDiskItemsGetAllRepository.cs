using Starter.Template.AOT.Api.Shared.DiskItem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

public interface IDiskItemsGetAllRepository
{
    Task<DiskItemEntity> GetByDriveIndexAsync(int driveIndex);
}
