namespace Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;

public interface IDiskDrivesGetAllRepository
{
    DriveInfo[] GetAllDrives();
}
