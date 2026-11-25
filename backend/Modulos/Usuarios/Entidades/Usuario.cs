using CampusParticular.Excepciones;

namespace CampusParticular.Modulos.Usuarios.Entidades;

public class Usuario
{
  public Guid Id { get; }
  public string CorreoElectronico { get; }
  public string HashPassword { get; private set; }
  public string Nombre { get; set; }
  public string Apellido { get; set; }
  public RolUsuario Rol { get; set; }
  public bool Activo { get; set; }
  public DateTimeOffset FechaCreado { get; }
  public DateTimeOffset FechaActualizado { get; set; }
  public DateTimeOffset? FechaDesactivado { get; set; }

  public Usuario(string correoElectronico, string hashPassword, string nombre, string apellido, RolUsuario rol)
  {
    Id = Guid.NewGuid();
    CorreoElectronico = correoElectronico;
    HashPassword = hashPassword;
    Nombre = nombre;
    Apellido = apellido;
    Rol = rol;
    Activo = true;
    FechaCreado = DateTimeOffset.UtcNow;
    FechaActualizado = FechaCreado;
  } 

  public bool EsAdministrador()  
  => Rol.Equals(RolUsuario.Admin); 

  public void ActualizarPassword(string nuevoHashPassword)
  {
    if (!Activo)
    {
      throw new AplicacionException
      (TipoError.Validacion,"No se puede modificar un usuario desactivado.");
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(nuevoHashPassword);
    HashPassword = nuevoHashPassword;
    FechaActualizado = DateTimeOffset.UtcNow;
  }

private static string ValidarCampo(string valor, string nombreCampo)
{
  if (string.IsNullOrWhiteSpace(valor))
    {
      throw new AplicacionException
        (TipoError.Validacion, "El Nombre es obligatorio"); 
    }
  return valor.Trim();
}

public bool verificarActivo()
  {
    if (!Activo)
    {
      throw new AplicacionException
        (TipoError.Validacion,"El usuario está desactivado");
    }
    return true; 
  }

public void ActualizarPerfil(string nombre, string apellido)
{ 
  if (verificarActivo())
    {
      Nombre = ValidarCampo(nombre,"Nombre"); 
      Apellido = ValidarCampo(apellido,"Apellido");
      FechaActualizado = DateTimeOffset.UtcNow;  
    }
}

public void CambiarRol(RolUsuario nuevoRol)
  {
    if (verificarActivo())
    {
      Rol=nuevoRol;
      FechaActualizado=DateTimeOffset.UtcNow; 
    }
  } 

public void Desactivar()
  {
    if(verificarActivo())
    {
        Activo=false; 
        FechaActualizado=DateTimeOffset.UtcNow; 
        FechaDesactivado=DateTimeOffset.UtcNow;
    }
  }

  public void Reactivar()
  {
    if (Activo == false)
    {
      Activo = true;
      FechaDesactivado=null; 
      FechaActualizado=DateTimeOffset.UtcNow; 
    } 
  }


}

