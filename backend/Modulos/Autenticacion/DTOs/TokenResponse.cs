using System;
namespace CampusParticular.Modulos.Autenticacion.DTOs;

public sealed record TokenResponse(
    string AccessToken,
    DateTimeOffset ExpiraEn);

