namespace CampusParticular.Modulos.Autenticacion.Seguridad;

public sealed record RefreshTokenGenerado(
    string Valor,
    string Hash,
    DateTimeOffset ExpiraEn);