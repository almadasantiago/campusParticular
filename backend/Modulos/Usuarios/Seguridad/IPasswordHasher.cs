namespace CampusParticular.Modulos.Usuarios.Seguridad;

public interface IPasswordHasher
{
    string Hashear(string password);

    bool Verificar(
        string passwordIngresada,
        string passwordHasheada);

}