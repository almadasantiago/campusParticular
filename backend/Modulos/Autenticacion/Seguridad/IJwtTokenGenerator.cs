using System;
using CampusParticular.Modulos.Usuarios.Entidades;

namespace CampusParticular.Modulos.Autenticacion.Seguridad;

public interface IJwtTokenGenerator
{
    TokenGenerado Generar (Usuario usuario); 
}
