using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber.NumberStringGetByNumberOutput))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
