namespace CampusParticular.Excepciones;

public sealed class AplicacionException : Exception
{
    public TipoError Tipo { get; }

    public AplicacionException(
        TipoError tipo,
        string mensaje)
        : base(mensaje)
    {
        Tipo = tipo;
    }

    public static AplicacionException CredencialesInvalidas()
    {
        return new AplicacionException(
            TipoError.NoAutorizado,
            "Las credenciales ingresadas no son válidas.");
    }
}
