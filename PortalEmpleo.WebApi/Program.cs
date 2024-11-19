using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PortalEmpleo.Domain.Contracts;
using PortalEmpleo.Domain.Services;
using PortalEmpleo.Infraestructure;
using PortalEmpleo.WebApi.Attributes;
using PortalEmpleo.Shared.InDTO;
using PortalEmpleo.Domain.Contracts.OfertaEmpleoRepository;
using PortalEmpleo.Domain.Contracts.PostulacionRepository;
using PortalEmpleo.Domain.Services.OfertaEmpleoRepository;
using PortalEmpleo.Domain.Services.PostulacionRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Configuración de la base de datos
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configuración de settings
builder.Services.Configure<UsuarioSettings>(
    builder.Configuration.GetSection("UsuarioSettings")
);

// Registro de servicios principales
builder.Services.AddScoped<IAccesoRepository, AccesoRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IConstantesService, ConstantesService>();
builder.Services.AddScoped<IOfertaEmpleoRepository, OfertaEmpleoRepository>();
builder.Services.AddScoped<IPostulacionRepository, PostulacionRepository>();

// Registro de filtros y atributos
builder.Services.AddScoped<AccesoAttribute>();
builder.Services.AddScoped<AutorizacionJwtAttribute>();
builder.Services.AddScoped<LogAttribute>();
builder.Services.AddScoped<ValidarModeloAttribute>();

// Configuración de filtros globales en los controladores
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LogAttribute>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Manejo global de excepciones
app.UseExceptionHandler("/Error");
app.UseHsts();

app.MapControllers();

app.Run();