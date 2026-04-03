using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DrivesGetAll;
using Starter.Template.AOT.Api.Features.Query.DiskItemsGetAllByDrive;
using Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(DrivesGetAllOutput))]
[JsonSerializable(typeof(List<DriveOutput>))]
[JsonSerializable(typeof(DiskItemsGetAllByDriveOutput))]
[JsonSerializable(typeof(DiskItemOutput))]
[JsonSerializable(typeof(List<DiskItemOutput>))]
[JsonSerializable(typeof(DiskItemGetByFolderOutput))]
[JsonSerializable(typeof(DiskItemFolderOutput))]
[JsonSerializable(typeof(List<DiskItemFolderOutput>))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
