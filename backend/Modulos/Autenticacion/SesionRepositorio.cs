using CampusParticular.Infraestructura.Persistencia;
using CampusParticular.Modulos.Usuarios.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace CampusParticular.Modulos.Autenticacion;

public sealed class SesionRepositorio(
    CampusDbContext dbContext)
    : ISesionRepositorio
{
    public void Agregar(UserSesion sesion)
    {
        dbContext.SesionesUsuario.Add(sesion);
    }

    public Task<UserSesion?> BuscarPorHashAsync(
        string hash,
        CancellationToken ct)
    {
        return dbContext.SesionesUsuario
            .Include(sesion => sesion.Usuario)
            .SingleOrDefaultAsync(
                sesion =>
                    sesion.RefreshTokenHash == hash,
                ct);
    }

    public async Task<IReadOnlyList<UserSesion>>
        BuscarActivasAsync(
            Guid usuarioId,
            CancellationToken ct)
    {
        DateTimeOffset ahora = DateTimeOffset.UtcNow;

        return await dbContext.SesionesUsuario
            .Where(sesion =>
                sesion.UsuarioId == usuarioId &&
                sesion.RevocadaEn == null &&
                sesion.ExpiraEn > ahora)
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<UserSesion>>
        BuscarFamiliaAsync(
            Guid familiaId,
            CancellationToken ct)
    {
        return await dbContext.SesionesUsuario
            .Where(sesion =>
                sesion.FamiliaId == familiaId)
            .ToListAsync(ct);
    }

    public async Task GuardarCambiosAsync(
        CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}