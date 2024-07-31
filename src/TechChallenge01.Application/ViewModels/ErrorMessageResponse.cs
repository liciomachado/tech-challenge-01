using System.Text.Json.Serialization;

namespace TechChallenge01.Application.ViewModels;

public record ErrorMessageResponse([property: JsonPropertyName("message")] string Message);
