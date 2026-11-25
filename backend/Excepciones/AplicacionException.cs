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
}