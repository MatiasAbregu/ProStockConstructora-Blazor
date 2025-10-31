using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD.Modelos;
using Microsoft.EntityFrameworkCore;

namespace BD
{
    public class AppDbContext : DbContext
    {
        public DbSet<ObraUsuario> ObraUsuarios { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ubicacion> Ubicaciones { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<TipoMaterial> TipoMateriales { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Recursos> MaterialesyMaquinas { get; set; }
        public DbSet<DetalleNotaDePedido> DetalleNotaDePedidos { get; set; }
        public DbSet<NotaDePedido> NotaDePedidos { get; set; }
        public DbSet<DetalleRemito> DetalleRemitos { get; set; }
        public DbSet<Remito> Remitos { get; set; }
        public DbSet<MovimientoDeposito> MovimientoDepositos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}