using System;
using System.Security.Cryptography.X509Certificates;
using CampusParticular.Infraestructura.Persistencia;
using CampusParticular.Modulos.Usuarios.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CampusParticular.Modulos.Usuarios.Persistencia;

public sealed class UsuarioRepositorio(CampusDbContext dbContext): IUsuarioRepositorio
{  
    public async Task GuardarCambiosAsync (CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct); 
    }
    public async Task<Usuario?> BuscarUsuarioPorIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.Usuarios.FindAsync(new object?[] {id}, ct); 

    }
    public async Task<Usuario?> BuscarUsuarioPorCorreoAsync(string correoElectronico, CancellationToken ct)
    {
        return await dbContext.Usuarios.
            AsNoTracking().FirstOrDefaultAsync(u => u.CorreoElectronico == correoElectronico,ct);
    }
    public async Task<Usuario?> BuscarUsuarioPorNombreOApellidoAsync(string Nombre, string Apellido, CancellationToken ct)
    {
        return await dbContext.Usuarios.
            AsNoTracking().
            FirstOrDefaultAsync(u => u.Nombre == Nombre || u.Apellido == Apellido,ct); 
    }
    public void AgregarUsuario(Usuario usuario) 
        => dbContext.Usuarios.Add(usuario);

  public async Task<IReadOnlyList<Usuario>> ListarUsuariosAsync( bool incluirDesactivados, CancellationToken ct)
{
    IQueryable<Usuario> consulta = dbContext.Usuarios.AsNoTracking();
    if (!incluirDesactivados)
    {
        consulta = consulta.Where(usuario => usuario.Activo);
    }
    return await consulta.ToListAsync(ct);
}

}
