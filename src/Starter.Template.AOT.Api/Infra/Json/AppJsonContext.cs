using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.Api.Features.Query.DiskItemsGetAll;
using Starter.Template.AOT.Api.Features.Query.DiskItemGetByFolder;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(List<DiskDrivesGetAllOutput>))]
[JsonSerializable(typeof(DiskItemsGetAllOutput))]
[JsonSerializable(typeof(DiskItemGetByFolderOutput))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
