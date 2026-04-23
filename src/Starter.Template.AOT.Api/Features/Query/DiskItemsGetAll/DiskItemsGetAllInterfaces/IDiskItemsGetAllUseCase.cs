namespace Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;

public interface IDiskItemsGetAllUseCase
{
    Task<DiskItemsGetAllOutput> ExecuteAsync(int driveIndex);
}
