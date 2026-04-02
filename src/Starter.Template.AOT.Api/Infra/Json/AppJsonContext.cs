using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.NumberStringGetByValue;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(NumberStringGetByValueOutput))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
