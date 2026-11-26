using Microsoft.EntityFrameworkCore; 
using CampusParticular.Modulos.Usuarios.Entidades;
using CampusParticular.Modulos.Autenticacion;


namespace CampusParticular.Infraestructura.Persistencia;

public sealed class CampusDbContext(DbContextOptions<CampusDbContext> opciones) : DbContext(opciones)
{
    
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UserSesion> SesionesUsuario => Set<UserSesion>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasIndex(usuario => usuario.CorreoElectronico)
            .IsUnique();
        modelBuilder.Entity<UserSesion>(sesion =>
        {
            sesion.HasKey(x => x.Id);
            sesion.HasIndex(x => x.RefreshTokenHash).IsUnique();
            sesion.HasIndex(x => x.UsuarioId);
            sesion.HasIndex(x => x.FamiliaId);
            sesion.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
