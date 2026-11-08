using System.Text.RegularExpressions;
using Fcg.Domain.Common;

namespace Fcg.Domain.Users;

public static partial class PasswordPolicy
{
    private static readonly Regex SecurePasswordRegex = CreateSecurePasswordRegex();

    public static void EnsureValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new DomainException("Senha é obrigatória.");

        if (password.Length < 8)
            throw new DomainException("Senha deve ter no mínimo 8 caracteres.");

        if (!SecurePasswordRegex.IsMatch(password))
            throw new DomainException("Senha deve conter letras, números e caracteres especiais.");
    }

    [GeneratedRegex(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$", RegexOptions.Compiled)]
    private static partial Regex CreateSecurePasswordRegex();
}
