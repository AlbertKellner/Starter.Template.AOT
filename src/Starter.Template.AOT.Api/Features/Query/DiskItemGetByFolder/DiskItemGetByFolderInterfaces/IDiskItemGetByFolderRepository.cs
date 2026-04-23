using Starter.Template.AOT.Api.Shared.DiskItem;

namespace Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

public interface IDiskItemGetByFolderRepository
{
    Task<DiskItemEntity?> GetAsync(int driveIndex, string folderPath);
}
