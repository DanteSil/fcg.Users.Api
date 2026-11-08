using FluentValidation;
using Fcg.Domain.Users;

namespace Fcg.Application.Users;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .Must(password =>
            {
                try
                {
                    PasswordPolicy.EnsureValid(password);
                    return true;
                }
                catch
                {
                    return false;
                }
            })
            .WithMessage("Senha deve ter no mínimo 8 caracteres, com letras, números e caracteres especiais.");
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Role)
            .Must(role => role is null || Enum.TryParse<UserRole>(role, true, out _))
            .WithMessage("Role inválida. Use User ou Admin.");
    }
}
