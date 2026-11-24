using CampusParticular.Modulos.Usuarios;
using CampusParticular.Modulos.Usuarios.Entidades;
using CampusParticular.Modulos.Usuarios.Persistencia;
using Microsoft.VisualBasic;

namespace CampusParticular.Modulos.Usuarios;

public sealed class UsuariosService
{
private readonly IUsuarioRepositorio _repositorio;
public UsuariosService(IUsuarioRepositorio repositorio)
{
  _repositorio = repositorio; 

}

public Task<Usuario?> BuscarPorIdAsync(Guid id, CancellationToken ct)
  {
    return _repositorio.buscarUsuarioPorIdAsync(id,ct);
  }
  

public void agregarUsuario(Usuario usuario)
  {
      _repositorio.AgregarUsuario(usuario);   

  }



}






