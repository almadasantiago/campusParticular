using CampusParticular.Infraestructura.Persistencia;
using CampusParticular.Modulos.Usuarios;
using CampusParticular.Modulos.Usuarios.Persistencia; 
using CampusParticular.Modulos.Usuarios.Seguridad;
using Microsoft.EntityFrameworkCore; 

var builder = WebApplication.CreateBuilder(args);
var rutaBase = Path.Combine(builder.Environment.ContentRootPath, "campus.db");
    builder.Services.AddDbContext<CampusDbContext>(
        opciones => opciones.UseSqlite($"Data Source={rutaBase}")); 

builder.Services.AddScoped<IUsuarioRepositorio,UsuarioRepositorio>(); 
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UsuariosService>(); 

var app = builder.Build();
await using var scope = app.Services.CreateAsyncScope();
var dbContext= scope.ServiceProvider.GetRequiredService<CampusDbContext>();
await dbContext.Database.EnsureCreatedAsync();

app.Run();
