using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;//Auntenticacion
using Microsoft.EntityFrameworkCore;//orm para poder manejar los modelos para pasarlos a la bd
using Microsoft.IdentityModel.Tokens;//indentificador
using System.Text;
using _20263recetario.Models;
using _20263recetario.Context;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configurar la segurudad de Identity(Microsoft)  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Conexion a la base de datos ApplicationDbContext corazon mo
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Configurar JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones => opciones.
    TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["LlaveJWT"]
            !)),
        ClockSkew = TimeSpan.Zero
    });

//registrar Automapper pasar el modelo al dto lo que queremos mostrar
    builder.Services.AddAutoMapper(CFG =>
    {
    }, typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())//scalar para pruebas borrarlo una vez echo
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
