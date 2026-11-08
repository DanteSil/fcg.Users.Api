using Microsoft.EntityFrameworkCore;
using Fcg.Domain.Interfaces;
using Fcg.Domain.Users;

namespace Fcg.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FcgDbContext _db;

    public UserRepository(FcgDbContext db) => _db = db;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _db.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
        await _db.Users.FirstOrDefaultAsync(x => x.Email.Value == email, cancellationToken);

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _db.Users.OrderBy(x => x.Name).ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default) =>
        await _db.Users.AddAsync(user, cancellationToken);

    public void Update(User user) => _db.Users.Update(user);

    public void Remove(User user) => _db.Users.Remove(user);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default) =>
        await _db.Users.AnyAsync(x => x.Email.Value == email, cancellationToken);
}
