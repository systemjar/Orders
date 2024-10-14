using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitOfWork.Implementatios;
using Orders.Backend.UnitOfWork.Interfaces;
using Orders.Backend.UnitOfWork.Interfaces.Orders.Backend.UnitsOfWork.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Para evitar las referencias circulares entre las tablas
builder.Services
    .AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Inyectamos el servicio para conectarse al SqlServer
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=CadenaSql"));

//Inyectamos el servicio de alimentador de base de datos, usamos Trasient porque solo se usa uno sola vez al cargar la aplicacion y por eso la colocamos en area de momoria no inmeditata
builder.Services.AddTransient<SeedDB>();

//Inyectamos Los genericos del Repositorio y del UnitOfWork, usamos Scoped porque se utilizan muchas veces en la aplicacion y la ponemos en un area de memoria accesible y rapida
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

//Inyectamos el repositorio y unidad de trabajo de paises
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

//Inyectamos el repositorio y unidad de trabajo de estados
builder.Services.AddScoped<IStatesRepository, StatesRepository>();
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();

var app = builder.Build();

//Para inyectar el SeedDB no se puede hacer directamente al program que es la clase que inyecta
SeedData(app);

void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedDB>();
        service!.SeedAsync().Wait();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Para configurar la seguridad del api
app.UseCors(x => x
    .AllowCredentials() //Cualquier credencial
    .AllowAnyHeader()   //Para permitir el envio de cualquier header
    .AllowAnyMethod()   //Cualquiera puede consumir cualquier metodo
    .SetIsOriginAllowed(origin => true)); //Si no se pone esta linea no va a funcionar

app.Run();