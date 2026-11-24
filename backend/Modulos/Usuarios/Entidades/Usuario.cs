using System;

namespace CampusParticular.Modulos.Usuarios.Entidades;

public class Usuario
{
  public Guid Id { get; }
  public string CorreoElectronico { get; }
  public string HashPassword { get; set; }
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

}

