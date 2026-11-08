using Fcg.Domain.Common;
using Fcg.Domain.Users.Events;

namespace Fcg.Domain.Users;

public class User : Entity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User()
    {
    }

    private User(string name, Email email, string passwordHash, UserRole role)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        CreatedAt = DateTime.UtcNow;
        Raise(new UserRegistered(Id, email.Value, CreatedAt));
    }

    public static User Register(string name, Email email, string passwordHash, UserRole role = UserRole.User)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Hash de senha é obrigatório.");

        return new User(name.Trim(), email, passwordHash, role);
    }

    public void UpdateProfile(string name, Email email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Nome é obrigatório.");

        Name = name.Trim();
        Email = email;
    }

    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Hash de senha é obrigatório.");

        PasswordHash = passwordHash;
    }

    public void ChangeRole(UserRole role) => Role = role;

    public bool IsAdmin => Role == UserRole.Admin;
}
