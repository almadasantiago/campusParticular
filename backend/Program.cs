using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CampusParticular.Infraestructura.Autenticacion;
using CampusParticular.Infraestructura.Autorizacion;
using CampusParticular.Infraestructura.Http;
using CampusParticular.Infraestructura.Persistencia;
using CampusParticular.Modulos.Autenticacion;
using CampusParticular.Modulos.Autenticacion.Seguridad;
using CampusParticular.Modulos.Usuarios;
using CampusParticular.Modulos.Usuarios.Entidades;
using CampusParticular.Modulos.Usuarios.Persistencia; 
using CampusParticular.Modulos.Usuarios.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var rutaBase = Path.Combine(builder.Environment.ContentRootPath, "campus.db");
    builder.Services.AddDbContext<CampusDbContext>(
        opciones => opciones.UseSqlite($"Data Source={rutaBase}")); 
JwtOptions jwtOptions = builder.Configuration
        .GetRequiredSection(JwtOptions.Seccion).Get<JwtOptions>() 
        ?? throw new InvalidOperationException("Falta la configuración JWT.");

builder.Services.AddOptions<JwtOptions>().
    BindConfiguration(JwtOptions.Seccion)
    .Validate( options => !string.IsNullOrWhiteSpace(options.Issuer),
    "Jwt: Issuer es obligatorio")
    .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), 
    "Jwt: Audience es obligatorio").
    Validate(options => !string.IsNullOrWhiteSpace(options.SigningKey)&&
    options.SigningKey.Length >= 32 , "Jwt: SigningKey debe tener al menos 32 caracteres .")
    .Validate(options => options.DuracionMinutos > 0, "La duración del token debe ser positiva").
    ValidateOnStart();
 builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtOptions.SigningKey)),
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                NameClaimType =
                    JwtRegisteredClaimNames.Sub,
                RoleClaimType =
                    ClaimTypes.Role
            };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        Politicas.GestionarUsuarios,
        policy => policy.RequireAuthenticatedUser().
            RequireRole(nameof(RolUsuario.Admin))).AddPolicy(
        Politicas.DictarCursos,
        policy => policy.
            RequireAuthenticatedUser().RequireRole(
                nameof(RolUsuario.Admin),
                nameof(RolUsuario.Profesor)));
builder.Services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>(); 
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<IUserActual,UserActual>();  
builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
builder.Services.AddScoped<IUsuarioRepositorio,UsuarioRepositorio>(); 
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UsuariosService>(); 
builder.Services.AddScoped<ISesionRepositorio,SesionRepositorio>();
builder.Services.AddProblemDetails();


var app = builder.Build();
await using var scope = app.Services.CreateAsyncScope();
var dbContext= scope.ServiceProvider.GetRequiredService<CampusDbContext>();
await dbContext.Database.EnsureCreatedAsync();
app.UseExceptionHandler(); 
app.UseAuthentication();
app.UseAuthorization(); 
app.Run();
