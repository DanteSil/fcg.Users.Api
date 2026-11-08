using Fcg.Application.Abstractions;
using Fcg.Application.Common;
using Fcg.Domain.Interfaces;
using Fcg.Domain.Users;

namespace Fcg.Application.Users;

public class AuthService
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository users,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _users = users;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        PasswordPolicy.EnsureValid(request.Password);
        var email = Email.Create(request.Email);

        if (await _users.EmailExistsAsync(email.Value, cancellationToken))
            throw new ConflictException("E-mail já cadastrado.");

        var user = User.Register(request.Name, email, _passwordHasher.Hash(request.Password));
        await _users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthResponse(_jwtTokenService.GenerateToken(user), user.ToDto());
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(request.Email);
        var user = await _users.GetByEmailAsync(email.Value, cancellationToken)
            ?? throw new UnauthorizedAppException("Credenciais inválidas.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAppException("Credenciais inválidas.");

        return new AuthResponse(_jwtTokenService.GenerateToken(user), user.ToDto());
    }
}
