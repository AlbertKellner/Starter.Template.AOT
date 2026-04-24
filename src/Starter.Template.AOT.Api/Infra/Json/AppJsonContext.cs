using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.DiskDrivesGetAll;
using Starter.Template.AOT.Api.Shared.DiskAnalysis;

namespace Starter.Template.AOT.Api.Infra.Json;

// TODO: Adicionar [JsonSerializable(typeof(...))] para cada tipo de Input/Output das Features
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(DiskDrivesGetAllOutput))]
[JsonSerializable(typeof(DiskItemEntity))]
[JsonSerializable(typeof(List<string>))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
