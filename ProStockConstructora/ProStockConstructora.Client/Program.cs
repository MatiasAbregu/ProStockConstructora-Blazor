using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProStockConstructora.Client;
using Servicios.ServiciosHttp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpServicio, HttpServicio>();
builder.Services.AddScoped<DatosSesion>();

await builder.Build().RunAsync();
