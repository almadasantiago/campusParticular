using CampusParticular.Excepciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CampusParticular.Infraestructura.Http;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, titulo, detalle) = exception switch
        {
            AplicacionException error => error.Tipo switch
            {
                TipoError.NoEncontrado =>
                    (
                        StatusCodes.Status404NotFound,
                        "Recurso no encontrado",
                        error.Message
                    ),

                TipoError.Conflicto =>
                    (
                        StatusCodes.Status409Conflict,
                        "Conflicto",
                        error.Message
                    ),

                TipoError.Validacion or TipoError.ReglaDeNegocio =>
                    (
                        StatusCodes.Status400BadRequest,
                        "Operación inválida",
                        error.Message
                    ),

                _ =>
                    (
                        StatusCodes.Status500InternalServerError,
                        "Error interno",
                        "Ocurrió un error inesperado."
                    )
            },

            ArgumentException =>
                (
                    StatusCodes.Status400BadRequest,
                    "Datos inválidos",
                    exception.Message
                ),

            _ =>
                (
                    StatusCodes.Status500InternalServerError,
                    "Error interno",
                    "Ocurrió un error inesperado."
                )
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Error no controlado");
        }

        httpContext.Response.StatusCode = status;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = detalle,
                Instance = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }
}