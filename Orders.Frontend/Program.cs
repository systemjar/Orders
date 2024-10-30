using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Orders.Frontend;
using Orders.Frontend.AuthenticationProviders;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Esta es la url que provee los servicios del backend
//Utilizamos AddSingleton para que solo haya um HttpCliente por toda la ejecucion del programa
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7192/") });

//Inyectamos el Repositorio porque el Frontend solo obtiene datos de repositorio
builder.Services.AddScoped<IRepository, Repository>();

//Inyectamos la libreria SweetAlert2
builder.Services.AddSweetAlert2();

//Servicio de autenticacion
builder.Services.AddAuthorizationCore();

//Proveedor de seguridad
/* Linea anterior
 * builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderTest>();*/

//Se reemplaza por esto
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x =>
x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x =>
x.GetRequiredService<AuthenticationProviderJWT>());

await builder.Build().RunAsync();