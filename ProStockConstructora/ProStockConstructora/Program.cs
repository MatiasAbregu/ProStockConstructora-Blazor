using BD;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProStockConstructora.Client;
using ProStockConstructora.Client.Pages;
using ProStockConstructora.Components;
using Repositorios.Implementaciones;
using Repositorios.Servicios;
using Servicios.ServiciosHttp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7283/") });


builder.Services.AddScoped<IHttpServicio, HttpServicio>();

// Estableciendo conexión 
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseMySql(builder.Configuration.GetConnectionString("ConexionDB"),
       new MariaDbServerVersion(new Version(10, 4, 32))));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IUsuarioServicio, UsuarioServicio>();
builder.Services.AddScoped<IObraServicio, ObraServicio>();
builder.Services.AddScoped<IDepositoServicio, DepositoServicio>();
builder.Services.AddScoped<IRolesServicio, RolesServicio>();
builder.Services.AddScoped<IRecursosServicio, RecursosServicio>();
builder.Services.AddScoped<INotaDePedidoServicio, NotaDePedidoServicio>();
builder.Services.AddScoped<IUnidadMedidaServicio, UnidadMedidaServicio>();
builder.Services.AddScoped<IHttpServicio, HttpServicio>();
builder.Services.AddScoped<IRemitoServicio, RemitoServicio>();

builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(navigationManager.BaseUri) };
});

builder.Services.AddServerSideBlazor().AddCircuitOptions(opt => { opt.DetailedErrors = true; });

builder.Services.AddScoped<DatosSesion>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ProStockConstructora.Client._Imports).Assembly);

app.Run();
