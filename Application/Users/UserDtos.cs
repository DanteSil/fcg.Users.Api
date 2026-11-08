using Fcg.Domain.Users;

namespace Fcg.Application.Users;

public record UserDto(Guid Id, string Name, string Email, string Role, DateTime CreatedAt);

public record RegisterUserRequest(string Name, string Email, string Password);

public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, UserDto User);

public record UpdateUserRequest(string Name, string Email, string? Role);

public static class UserMappings
{
    public static UserDto ToDto(this User user) =>
        new(user.Id, user.Name, user.Email.Value, user.Role.ToString(), user.CreatedAt);
}
