namespace ConsoleApp.Models;

public record AppSettings(
    string? SwaggerYamlUrl = null,
    string? SwaggerYamlFile = null,
    string? SwaggerJsonUrl = null,
    string? SwaggerJsonFile = null,
    string? ClassName = null,
    string? Namespace = null
    );