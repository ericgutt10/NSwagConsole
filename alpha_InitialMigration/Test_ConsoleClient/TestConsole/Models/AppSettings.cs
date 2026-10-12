namespace TestConsole.Models;

public record AppSettings(
    string? CHREnvironmentURL = null,
    string? ClientSecretKey = null
    );