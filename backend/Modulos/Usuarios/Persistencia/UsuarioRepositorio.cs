using System;
using System.Security.Cryptography.X509Certificates;
using CampusParticular.Infraestructura.Persistencia;
using CampusParticular.Modulos.Usuarios.Entidades;

namespace CampusParticular.Modulos.Usuarios.Persistencia;

public sealed class UsuarioRepositorio(CampusDbContext dbContext): IUsuarioRepositorio
{  
    public void AgregarUsuario(Usuario usuario) 
        => dbContext.Usuarios.Add(usuario);

    public async Task<Usuario?> buscarUsuarioPorIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.Usuarios.FindAsync(new object?[] {id}, ct); 

    }

    public async Task GuardarCambiosAsync (CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct); 
    }

}
