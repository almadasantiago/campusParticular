using CampusParticular.Modulos.Usuarios.Entidades;

namespace CampusParticular.Modulos.Autenticacion;

public sealed class UserSesion
{
    public Guid Id { get; private set; }

    public Guid UsuarioId { get; private set; }

    public Usuario Usuario { get; private set; } = null!;

    public Guid FamiliaId { get; private set; }

    public string RefreshTokenHash { get; private set; } = null!;

    public DateTimeOffset CreadaEn { get; private set; }

    public DateTimeOffset ExpiraEn { get; private set; }

    public DateTimeOffset? RevocadaEn { get; private set; }

    public Guid? ReemplazadaPorId { get; private set; }

    public string? MotivoRevocacion { get; private set; }

    private UserSesion()
    {
    }

    public UserSesion(
        Guid usuarioId,
        Guid familiaId,
        string refreshTokenHash,
        DateTimeOffset expiraEn)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        FamiliaId = familiaId;
        RefreshTokenHash = refreshTokenHash;
        CreadaEn = DateTimeOffset.UtcNow;
        ExpiraEn = expiraEn;
    }

    public bool EstaActiva =>
        RevocadaEn is null &&
        ExpiraEn > DateTimeOffset.UtcNow;

    public void Revocar(
        string motivo,
        Guid? reemplazadaPorId = null)
    {
        if (RevocadaEn is not null)
        {
            return;
        }

        RevocadaEn = DateTimeOffset.UtcNow;
        MotivoRevocacion = motivo;
        ReemplazadaPorId = reemplazadaPorId;
    }
}