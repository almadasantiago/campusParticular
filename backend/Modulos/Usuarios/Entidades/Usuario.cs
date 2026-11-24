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

  public Boolean soyAdministrador()
   => this.Rol.Equals(RolUsuario.Admin); 

  public void ActualizarPassword(string nuevoHashPassword)
  {
    if (!Activo)
    {
      throw new InvalidOperationException("No se puede modificar un usuario desactivado.");
    }

    ArgumentException.ThrowIfNullOrWhiteSpace(nuevoHashPassword);
    HashPassword = nuevoHashPassword;
    FechaActualizado = DateTimeOffset.UtcNow;
  }

public void ActualizarPerfil(string nombre, string apellido)
{
  if (!Activo)
  {
    throw new InvalidOperationException(
      "No se puede modificar un usuario desactivado.");
  }
  if (string.IsNullOrWhiteSpace(nombre))
  {
        throw new ArgumentException(
            "El nombre es obligatorio.",
            nameof(nombre));
  }

    if (string.IsNullOrWhiteSpace(apellido))
    {
        throw new ArgumentException(
            "El apellido es obligatorio.",
            nameof(apellido));
    }

    Nombre = nombre.Trim();
    Apellido = apellido.Trim();
    FechaActualizado = DateTimeOffset.UtcNow;
}


}

