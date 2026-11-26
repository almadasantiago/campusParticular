using System;

namespace CampusParticular.Modulos.Autenticacion.Seguridad;

public interface IRefreshTokenGenerator
{
    RefreshTokenGenerado Generar();

    string CalcularHash(string refreshToken);
}