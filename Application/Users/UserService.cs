using Fcg.Application.Common;
using Fcg.Domain.Interfaces;
using Fcg.Domain.Users;

namespace Fcg.Application.Users;

public class UserService
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository users, IUnitOfWork unitOfWork)
    {
        _users = users;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _users.GetAllAsync(cancellationToken);
        return users.Select(u => u.ToDto()).ToList();
    }

    public async Task<UserDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        return user.ToDto();
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var email = Email.Create(request.Email);
        var existing = await _users.GetByEmailAsync(email.Value, cancellationToken);
        if (existing is not null && existing.Id != id)
            throw new ConflictException("E-mail já cadastrado.");

        user.UpdateProfile(request.Name, email);

        if (!string.IsNullOrWhiteSpace(request.Role) &&
            Enum.TryParse<UserRole>(request.Role, true, out var role))
        {
            user.ChangeRole(role);
        }

        _users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return user.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        _users.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
