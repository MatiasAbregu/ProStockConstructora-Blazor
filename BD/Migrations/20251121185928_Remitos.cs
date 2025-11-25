using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Migrations
{
    /// <inheritdoc />
    public partial class Remitos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaLimite",
                table: "Remitos");

            migrationBuilder.DropColumn(
                name: "NombreTransportista",
                table: "Remitos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLimite",
                table: "Remitos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NombreTransportista",
                table: "Remitos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
