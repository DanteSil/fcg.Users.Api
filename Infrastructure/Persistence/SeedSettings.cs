namespace Fcg.Infrastructure.Persistence;

public class SeedSettings
{
    public const string SectionName = "Seed";

    public string AdminName { get; set; } = "Administrador";
    public string AdminEmail { get; set; } = "admin@fcg.com";
    public string AdminPassword { get; set; } = string.Empty;
}
