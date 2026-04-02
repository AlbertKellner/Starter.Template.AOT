using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.NumberToString;

namespace Starter.Template.AOT.Api.Infra.Json;

[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(NumberToStringOutput))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
