using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.NumberStringGetByNumber;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(NumberStringGetByNumberOutput))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
