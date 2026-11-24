using System;
using CampusParticular.Modulos.Usuarios.Entidades;

namespace CampusParticular.Modulos.Usuarios.Persistencia;

public interface IUsuarioRepositorio
{  
    void AgregarUsuario(Usuario usuario); 
    Task<Usuario?> BuscarUsuarioPorIdAsync(Guid IdUsuario, CancellationToken ct); 
    Task<Usuario?> BuscarUsuarioPorCorreoAsync(string correoElectronico, CancellationToken ct);
    Task<Usuario?> BuscarUsuarioPorNombreOApellidoAsync(string Nombre, string Apellido, CancellationToken ct); 
    Task GuardarCambiosAsync(CancellationToken ct); 
    Task<IReadOnlyList<Usuario>> ListarUsuariosAsync( bool incluirDesactivados, CancellationToken ct); 


}
