using System.Security.Cryptography;
using System.Text;
using CampusParticular.Modulos.Autenticacion.Seguridad;
using Microsoft.IdentityModel.Tokens;

namespace CampusParticular.Infraestructura.Autenticacion;

public sealed class RefreshTokenGenerator
    : IRefreshTokenGenerator
{
    private static readonly TimeSpan Duracion =
        TimeSpan.FromDays(30);

    public RefreshTokenGenerado Generar()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);

        string valor = Base64UrlEncoder.Encode(bytes);
        string hash = CalcularHash(valor);

        return new RefreshTokenGenerado(
            valor,
            hash,
            DateTimeOffset.UtcNow.Add(Duracion));
    }

    public string CalcularHash(string refreshToken)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(refreshToken);
        byte[] hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}