using System;

namespace CampusParticular.Infraestructura.Autenticacion;

public sealed class JwtOptions
{
    public const string Seccion = "Jwt"; 
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string SigningKey { get; init; }
    public int DuracionMinutos { get; init; } = 15;  
}
