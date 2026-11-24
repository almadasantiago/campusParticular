using System;
using CampusParticular.Modulos.Usuarios.Entidades;

namespace CampusParticular.Modulos.Usuarios.Persistencia;

public interface IUsuarioRepositorio
{  
    void AgregarUsuario(Usuario usuario); 
    Task<Usuario?> buscarUsuarioPorIdAsync(Guid IdUsuario, CancellationToken ct); 
    Task GuardarCambiosAsync(CancellationToken ct); 

}
