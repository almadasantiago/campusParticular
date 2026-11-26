using CampusParticular.Excepciones;
using CampusParticular.Infraestructura.Autenticacion;
using CampusParticular.Modulos.Autenticacion;
using CampusParticular.Modulos.Usuarios;
using CampusParticular.Modulos.Usuarios.Entidades;
using CampusParticular.Modulos.Usuarios.Persistencia;
using CampusParticular.Modulos.Usuarios.Seguridad;
using Microsoft.VisualBasic;

namespace CampusParticular.Modulos.Usuarios;

public sealed class UsuariosService
{
private readonly IUsuarioRepositorio _repositorio;
private readonly IPasswordHasher _passwordHasher;
private readonly IUserActual _userActual; 
public UsuariosService(IUsuarioRepositorio repositorio, IPasswordHasher passwordHasher, IUserActual usuarioActual)
{
  _repositorio = repositorio; 
  _passwordHasher = passwordHasher;
  _userActual = usuarioActual; 
}
string noExiste = "El usuario no existe"; 
public Task<Usuario?> BuscarPorIdAsync(Guid id, CancellationToken ct)
  {
    return _repositorio.BuscarUsuarioPorIdAsync(id,ct);
  }

private async Task<Usuario> GetUsuarioAsync(Guid id, CancellationToken ct)
  {
    Usuario? usuario = await _repositorio.BuscarUsuarioPorIdAsync(id,ct); 
    return usuario ?? throw new AplicacionException
      ( TipoError.NoEncontrado,$"El usuario con ID '{id}' no existe ");
  } 

public Task<Usuario?> BuscarPorCorreoAsync (string correoElectronico, CancellationToken ct)
  {
    return _repositorio.BuscarUsuarioPorCorreoAsync(correoElectronico,ct); 
  }

public Task<Usuario?> BuscarPorNombreOApellidoAsync (string Nombre,string Apellido, CancellationToken ct)
  {
    return _repositorio.BuscarUsuarioPorNombreOApellidoAsync(Nombre,Apellido,ct); 
  }

public async Task<Usuario> CrearUsuarioAsync(string correoElectronico,string password,string nombre, string apellido, CancellationToken ct)
  {
    ValidarPassword(password);
    string correo=correoElectronico.Trim().ToLowerInvariant(); 
     Usuario? usuarioExistente = await BuscarPorCorreoAsync(correo, ct);
     
    if (usuarioExistente is not null)
      {
        throw new AplicacionException
          (TipoError.Conflicto," Ya se encuentra registrado un usuario con ese correo "); 
      }
    
    string hashPassword = _passwordHasher.Hashear(password);
    Usuario userNuevo = new Usuario(correo,hashPassword,nombre.Trim(),apellido.Trim(),RolUsuario.Estudiante); 
    _repositorio.AgregarUsuario(userNuevo); 
    await _repositorio.GuardarCambiosAsync(ct);
    return userNuevo; 
  }

  public Task<IReadOnlyList<Usuario>>ListarAsync(bool incluirDesactivados,CancellationToken ct)
  {
    return _repositorio.ListarUsuariosAsync(incluirDesactivados,ct);  
  }

  public async Task CambiarPasswordAsync(Guid Id, string PasswordActual, 
      string PasswordNueva, CancellationToken ct)
  {
    Usuario? usuario = await _repositorio.BuscarUsuarioPorIdAsync(Id,ct); 
    if (usuario is null)
    {
      throw new AplicacionException
        (TipoError.NoEncontrado,noExiste);  
    }

    if (!_passwordHasher.Verificar(PasswordActual, usuario.HashPassword))
    {
      throw new AplicacionException
        (TipoError.Validacion,"La contraseña actual es incorrecta.");
    }

    ValidarPassword(PasswordNueva);
    string nuevoHashPassword = _passwordHasher.Hashear(PasswordNueva);
    usuario.ActualizarPassword(nuevoHashPassword);
    await _repositorio.GuardarCambiosAsync(ct);
  }

private static void ValidarPassword(string password)
{
  if (string.IsNullOrWhiteSpace(password))
  {
    throw new AplicacionException
      (TipoError.Validacion, "La contraseña es obligatoria ");  
  }

  if (password.Length < 6)
  {
    throw new AplicacionException(TipoError.ReglaDeNegocio,
      "La contraseña debe tener al menos 6 caracteres.");
  }

}

public async Task<Usuario> ActualizarPerfilAsync(Guid id, string nombre, string apellido, CancellationToken ct)
{
    Usuario? usuario = await _repositorio.BuscarUsuarioPorIdAsync(id, ct);

    if (usuario is null)
    {
      throw new AplicacionException
        (TipoError.NoEncontrado,noExiste); 
    }

    usuario.ActualizarPerfil(nombre, apellido);
    await _repositorio.GuardarCambiosAsync(ct);

    return usuario;
}

public async Task CambiarRolAsync(Guid usuarioId, RolUsuario nuevoRol, CancellationToken ct)
  {
    if (_userActual.Id == usuarioId)
    {
      throw new AplicacionException
        (TipoError.ReglaDeNegocio," El usuario actual no puede cambiar su propio rol ");
    }

    Usuario usuario= await GetUsuarioAsync(usuarioId, ct);
    usuario.CambiarRol(nuevoRol); 
    await _repositorio.GuardarCambiosAsync(ct);  

  }



}






