using System;

namespace CampusParticular.Modulos.Autenticacion.Seguridad;

public sealed record TokenGenerado(
    string AccessToken,
    DateTimeOffset ExpiraEn);