using System;

namespace CampusParticular.Modulos.Autenticacion;

public interface IUserActual
{  
    Guid Id { get; }
    bool EstaAutenticado { get; }

}
