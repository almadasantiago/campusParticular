using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CampusParticular.Modulos.Autenticacion.Seguridad;
using CampusParticular.Modulos.Usuarios.Entidades;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CampusParticular.Infraestructura.Autenticacion;

public sealed class JwtTokenGenerator(
    IOptions<JwtOptions> options)
    : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public TokenGenerado Generar(Usuario usuario)
    {
        DateTimeOffset ahora = DateTimeOffset.UtcNow;
        DateTimeOffset expiracion =
            ahora.AddMinutes(_options.DuracionMinutos);

        Claim[] claims =
        [
            new Claim(
                JwtRegisteredClaimNames.Sub,
                usuario.Id.ToString()),

            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.Id.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Email,
                usuario.CorreoElectronico),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        ];

        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SigningKey));

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: ahora.UtcDateTime,
            expires: expiracion.UtcDateTime,
            signingCredentials: credenciales);

        string accessToken =
            new JwtSecurityTokenHandler()
                .WriteToken(token);

        return new TokenGenerado(
            accessToken,
            expiracion);
    }
}