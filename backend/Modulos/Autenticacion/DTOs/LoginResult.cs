using System;

namespace CampusParticular.Modulos.Autenticacion.DTOs;

public sealed record LoginResult(
    string AccessToken,
    DateTimeOffset AccessTokenExpiraEn,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiraEn);