using Microsoft.AspNetCore.Identity;

namespace CampusParticular.Modulos.Usuarios.Seguridad;

public sealed class PasswordHasher : IPasswordHasher
{
    private readonly Microsoft.AspNetCore.Identity.PasswordHasher<object> _hasher = new();

    public string Hashear(string password)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        return _hasher.HashPassword(
            user: null!,
            password);
    }

    public bool Verificar(
        string passwordIngresada,
        string passwordHasheada)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordIngresada);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHasheada);

        PasswordVerificationResult resultado =
            _hasher.VerifyHashedPassword(
                user: null!,
                hashedPassword: passwordHasheada,
                providedPassword: passwordIngresada);

        return resultado is
            PasswordVerificationResult.Success or
            PasswordVerificationResult.SuccessRehashNeeded;
    }
}
