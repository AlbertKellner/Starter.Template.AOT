namespace Starter.Template.AOT.Api.Features.Query.DiskStructureGetByFolder;

public interface IDiskStructureGetByFolderUseCase
{
    Task<DiskStructureGetByFolderOutput?> ExecuteAsync(string selectedDrive, string selectedFolder);
}
