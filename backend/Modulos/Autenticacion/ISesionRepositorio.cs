using System;
using CampusParticular.Modulos.Autenticacion; 

namespace CampusParticular.Modulos.Usuarios.Persistencia;

public interface ISesionRepositorio
{
    void Agregar( UserSesion sesion);

    Task<UserSesion?> BuscarPorHashAsync(
        string refreshTokenHash,
        CancellationToken ct);

    Task<IReadOnlyList<UserSesion>> BuscarActivasAsync(
        Guid usuarioId,
        CancellationToken ct);

    Task<IReadOnlyList<UserSesion>> BuscarFamiliaAsync(
        Guid familiaId,
        CancellationToken ct);

    Task GuardarCambiosAsync(CancellationToken ct);
}