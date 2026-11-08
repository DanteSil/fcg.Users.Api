using System.Text.RegularExpressions;
using Fcg.Domain.Common;

namespace Fcg.Domain.Users;

public sealed partial class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex = CreateEmailRegex();

    public string Value { get; private set; } = string.Empty;

    private Email()
    {
    }

    private Email(string value) => Value = value;

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("E-mail é obrigatório.");

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalized))
            throw new DomainException("Formato de e-mail inválido.");

        return new Email(normalized);
    }

    public bool Equals(Email? other) => other is not null && Value == other.Value;

    public override bool Equals(object? obj) => obj is Email other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex CreateEmailRegex();
}
