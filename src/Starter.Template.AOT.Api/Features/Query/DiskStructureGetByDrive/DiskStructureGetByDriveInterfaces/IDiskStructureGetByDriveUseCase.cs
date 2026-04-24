namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByDrive;

public interface IDiskStructureGetByDriveUseCase
{
    Task<DiskStructureGetByDriveOutput> ExecuteAsync(string selectedDrive);
}
