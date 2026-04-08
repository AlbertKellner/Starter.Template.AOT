using Starter.Template.AOT.Api.Features.Query.NumberToStringGet;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Starter.Template.AOT.Api.Infra.Json;

// TODO: Adicionar [JsonSerializable(typeof(...))] para cada tipo de Input/Output das Features
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(NumberToStringGetOutput))]
[JsonSerializable(typeof(ProblemDetails))]
[JsonSerializable(typeof(ValidationProblemDetails))]
internal sealed partial class AppJsonContext : JsonSerializerContext { }
