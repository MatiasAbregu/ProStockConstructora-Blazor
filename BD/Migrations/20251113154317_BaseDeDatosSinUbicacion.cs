using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BD.Migrations
{
    /// <inheritdoc />
    public partial class BaseDeDatosSinUbicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreEmpresa = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CUIT = table.Column<string>(type: "varchar(20)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazonSocial = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Celular = table.Column<string>(type: "varchar(80)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(120)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreRol = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoMateriales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMateriales", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UnidadMedidas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Simbolo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadMedidas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoObra = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreObra = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Obras_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreUsuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Contrasena = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmpresaId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoISO = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UnidadMedidaId = table.Column<long>(type: "bigint", nullable: true),
                    TipoMaterialId = table.Column<long>(type: "bigint", nullable: true),
                    Descripcion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recursos_TipoMateriales_TipoMaterialId",
                        column: x => x.TipoMaterialId,
                        principalTable: "TipoMateriales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recursos_UnidadMedidas_UnidadMedidaId",
                        column: x => x.UnidadMedidaId,
                        principalTable: "UnidadMedidas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Depositos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CodigoDeposito = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NombreDeposito = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDeposito = table.Column<int>(type: "int", nullable: false),
                    ObraId = table.Column<long>(type: "bigint", nullable: false),
                    Domicilio = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Depositos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Depositos_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ObraUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ObraId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObraUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObraUsuarios_Obras_ObraId",
                        column: x => x.ObraId,
                        principalTable: "Obras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObraUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RolesUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    RolId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolesUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DepositosUsuario",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositosUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositosUsuario_Depositos_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositosUsuario_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NotaDePedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroNotaPedido = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DepositoOrigenId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoDestinoId = table.Column<long>(type: "bigint", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotaDePedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotaDePedidos_Depositos_DepositoDestinoId",
                        column: x => x.DepositoDestinoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotaDePedidos_Depositos_DepositoOrigenId",
                        column: x => x.DepositoOrigenId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaterialesyMaquinasId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoId = table.Column<long>(type: "bigint", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Depositos_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Recursos_MaterialesyMaquinasId",
                        column: x => x.MaterialesyMaquinasId,
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetalleNotaDePedidos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NotaDePedidoId = table.Column<long>(type: "bigint", nullable: false),
                    MaterialesyMaquinasId = table.Column<long>(type: "bigint", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleNotaDePedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDePedidos_NotaDePedidos_NotaDePedidoId",
                        column: x => x.NotaDePedidoId,
                        principalTable: "NotaDePedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleNotaDePedidos_Recursos_MaterialesyMaquinasId",
                        column: x => x.MaterialesyMaquinasId,
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Remitos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroRemito = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NotaDePedidoId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoOrigenId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoId = table.Column<long>(type: "bigint", nullable: false),
                    DepositoDestinoId = table.Column<long>(type: "bigint", nullable: false),
                    DestinoId = table.Column<long>(type: "bigint", nullable: false),
                    EstadoRemito = table.Column<int>(type: "int", nullable: false),
                    NombreTransportista = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEmision = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaRecepcion = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Remitos_Depositos_DepositoId",
                        column: x => x.DepositoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remitos_Depositos_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Depositos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Remitos_NotaDePedidos_NotaDePedidoId",
                        column: x => x.NotaDePedidoId,
                        principalTable: "NotaDePedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DetalleRemitos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RemitoId = table.Column<long>(type: "bigint", nullable: false),
                    DetalleNotaDePedidoId = table.Column<long>(type: "bigint", nullable: false),
                    CantidadDespachada = table.Column<int>(type: "int", nullable: false),
                    CantidadRecibida = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetalleRemitos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetalleRemitos_DetalleNotaDePedidos_DetalleNotaDePedidoId",
                        column: x => x.DetalleNotaDePedidoId,
                        principalTable: "DetalleNotaDePedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleRemitos_Remitos_RemitoId",
                        column: x => x.RemitoId,
                        principalTable: "Remitos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "NombreRol" },
                values: new object[,]
                {
                    { -3L, "JEFEDEDEPOSITO" },
                    { -2L, "JEFEDEOBRA" },
                    { -1L, "ADMINISTRADOR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_CodigoDeposito",
                table: "Depositos",
                column: "CodigoDeposito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Depositos_ObraId",
                table: "Depositos",
                column: "ObraId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositosUsuario_DepositoId",
                table: "DepositosUsuario",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositosUsuario_UsuarioId_DepositoId",
                table: "DepositosUsuario",
                columns: new[] { "UsuarioId", "DepositoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDePedidos_MaterialesyMaquinasId",
                table: "DetalleNotaDePedidos",
                column: "MaterialesyMaquinasId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleNotaDePedidos_NotaDePedidoId",
                table: "DetalleNotaDePedidos",
                column: "NotaDePedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleRemitos_DetalleNotaDePedidoId",
                table: "DetalleRemitos",
                column: "DetalleNotaDePedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_DetalleRemitos_RemitoId",
                table: "DetalleRemitos",
                column: "RemitoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_CUIT",
                table: "Empresa",
                column: "CUIT",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotaDePedidos_DepositoDestinoId",
                table: "NotaDePedidos",
                column: "DepositoDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaDePedidos_DepositoOrigenId",
                table: "NotaDePedidos",
                column: "DepositoOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_NotaDePedidos_NumeroNotaPedido",
                table: "NotaDePedidos",
                column: "NumeroNotaPedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obras_CodigoObra",
                table: "Obras",
                column: "CodigoObra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Obras_EmpresaId",
                table: "Obras",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ObraUsuarios_ObraId_UsuarioId",
                table: "ObraUsuarios",
                columns: new[] { "ObraId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ObraUsuarios_UsuarioId",
                table: "ObraUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_CodigoISO",
                table: "Recursos",
                column: "CodigoISO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_TipoMaterialId",
                table: "Recursos",
                column: "TipoMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_UnidadMedidaId",
                table: "Recursos",
                column: "UnidadMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_Remitos_DepositoId",
                table: "Remitos",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Remitos_DestinoId",
                table: "Remitos",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Remitos_NotaDePedidoId",
                table: "Remitos",
                column: "NotaDePedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Remitos_NumeroRemito",
                table: "Remitos",
                column: "NumeroRemito",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NombreRol",
                table: "Roles",
                column: "NombreRol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsuarios_RolId",
                table: "RolesUsuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsuarios_UsuarioId_RolId",
                table: "RolesUsuarios",
                columns: new[] { "UsuarioId", "RolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_DepositoId",
                table: "Stocks",
                column: "DepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_MaterialesyMaquinasId",
                table: "Stocks",
                column: "MaterialesyMaquinasId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EmpresaId",
                table: "Usuarios",
                column: "EmpresaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositosUsuario");

            migrationBuilder.DropTable(
                name: "DetalleRemitos");

            migrationBuilder.DropTable(
                name: "ObraUsuarios");

            migrationBuilder.DropTable(
                name: "RolesUsuarios");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "DetalleNotaDePedidos");

            migrationBuilder.DropTable(
                name: "Remitos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Recursos");

            migrationBuilder.DropTable(
                name: "NotaDePedidos");

            migrationBuilder.DropTable(
                name: "TipoMateriales");

            migrationBuilder.DropTable(
                name: "UnidadMedidas");

            migrationBuilder.DropTable(
                name: "Depositos");

            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
