using System;

namespace CampusParticular.Modulos.Autenticacion.DTOs;

public sealed record LoginRequest(
    string CorreoElectrónico,
    string Password);
