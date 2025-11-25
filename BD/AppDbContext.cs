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
        public DbSet<DepositoUsuario> DepositosUsuario { get; set; }
        public DbSet<Deposito> Depositos { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolesUsuario> RolesUsuarios { get; set; }
        public DbSet<UnidadMedida> UnidadMedidas { get; set; }
        public DbSet<TipoMaterial> TipoMateriales { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Recursos> Recursos { get; set; }
        public DbSet<DetalleNotaDePedido> DetalleNotaDePedidos { get; set; }
        public DbSet<NotaDePedido> NotaDePedidos { get; set; }
        public DbSet<DetalleRemito> DetalleRemitos { get; set; }
        public DbSet<Remito> Remitos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotaDePedido>()
                .HasOne(n => n.DepositoOrigen)
                .WithMany()
                .HasForeignKey(n => n.DepositoOrigenId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<NotaDePedido>()
                .HasOne(n => n.DepositoDestino)
                .WithMany()
                .HasForeignKey(n => n.DepositoDestinoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = -1, NombreRol = "ADMINISTRADOR"},
                new Rol { Id = -2, NombreRol = "JEFEDEOBRA" },
                new Rol { Id = -3, NombreRol = "JEFEDEDEPOSITO" }
                );
        }
    }
}