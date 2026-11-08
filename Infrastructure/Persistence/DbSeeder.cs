using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Fcg.Application.Abstractions;
using Fcg.Domain.Users;

namespace Fcg.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var seed = scope.ServiceProvider.GetRequiredService<IOptions<SeedSettings>>().Value;
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");

        await db.Database.MigrateAsync();

        var email = Email.Create(seed.AdminEmail);
        var exists = await db.Users.AnyAsync(x => x.Email.Value == email.Value);
        if (exists)
            return;

        var admin = User.Register(seed.AdminName, email, hasher.Hash(seed.AdminPassword), UserRole.Admin);
        await db.Users.AddAsync(admin);
        await db.SaveChangesAsync();
        logger.LogInformation("Admin seed criado: {Email}", email.Value);
    }
}
