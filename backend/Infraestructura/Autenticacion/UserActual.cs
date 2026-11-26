using System;
using System.Security.Claims;
using CampusParticular.Modulos.Autenticacion;

namespace CampusParticular.Infraestructura.Autenticacion;

public sealed class UserActual( IHttpContextAccessor accessor) : IUserActual  
{
    private ClaimsPrincipal Principal => 
        accessor.HttpContext?.User 
            ?? throw new InvalidOperationException(" No existe un contexto HTTP"); 

    public bool EstaAutenticado => Principal.Identity?.IsAuthenticated == true; 
       public Guid Id
    {
        get
        {
            string? claim = Principal.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(claim, out Guid id))
            {
                throw new InvalidOperationException(
                    "La identidad autenticada no contiene " +
                    "un identificador válido.");
            }

            return id;
        }
    }
}
