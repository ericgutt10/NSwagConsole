namespace WorkerService1.Models;

public record AppSettings(
    string? CHREnvironmentURL = null,
    string? ClientSecretKey = null
    );