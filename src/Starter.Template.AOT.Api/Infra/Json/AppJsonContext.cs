using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Starter.Template.AOT.Api.Features.Query.NumberGetLabel;
using Starter.Template.AOT.Api.Features.Query.NumberGetText;

namespace Starter.Template.AOT.Api.Infra.Json;

// TODO: Adicionar [JsonSerializable(typeof(...))] para cada tipo de Input/Output das Features
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
[JsonSerializable(typeof(NumberGetLabelOutput))]
[JsonSerializable(typeof(NumberGetTextOutput))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
