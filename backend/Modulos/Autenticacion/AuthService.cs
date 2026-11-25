using System;
using CampusParticular.Excepciones;
using CampusParticular.Modulos.Autenticacion.DTOs;
using CampusParticular.Modulos.Autenticacion.Seguridad;
using CampusParticular.Modulos.Usuarios.Entidades;
using CampusParticular.Modulos.Usuarios.Persistencia;
using CampusParticular.Modulos.Usuarios.Seguridad;

namespace CampusParticular.Modulos.Autenticacion;

public sealed class AuthService(
    IUsuarioRepositorio usuarios, 
    IPasswordHasher passwordHasher, 
    IJwtTokenGenerator jwtTokenGenerator)
{
    public async Task<TokenResponse> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        string correo= request.CorreoElectrónico
        .Trim().ToLowerInvariant();    

        Usuario? usuario = await usuarios.BuscarUsuarioPorCorreoAsync(correo,ct); 
        bool credencialesValidas = ((usuario is not null) &&
            (passwordHasher.Verificar(request.Password,usuario.HashPassword))); 
        
        if (!credencialesValidas)
        {
            throw new AplicacionException
            (TipoError.Validacion,"Correo electronico o contraseña incorrectas");
        }
         if (!usuario!.Activo)
        {
            throw new AplicacionException(
                TipoError.Validacion,
                "El usuario está desactivado.");
        }
        
        TokenGenerado token = jwtTokenGenerator.Generar(usuario); 
        return new TokenResponse(token.AccessToken,token.ExpiraEn); 
    }
}
