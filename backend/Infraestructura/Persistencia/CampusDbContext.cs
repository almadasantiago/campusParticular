using Microsoft.EntityFrameworkCore; 
using CampusParticular.Modulos.Usuarios.Entidades; 

namespace CampusParticular.Infraestructura.Persistencia;

public sealed class CampusDbContext(DbContextOptions<CampusDbContext> opciones) : DbContext(opciones)
{
    
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.CorreoElectronico)
            .IsUnique();
    }


}
