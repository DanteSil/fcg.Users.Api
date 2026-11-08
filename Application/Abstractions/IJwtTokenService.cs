using Fcg.Domain.Users;

namespace Fcg.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
