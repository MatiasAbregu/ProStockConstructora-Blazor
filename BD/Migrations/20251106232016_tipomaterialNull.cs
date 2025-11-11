using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Migrations
{
    /// <inheritdoc />
    public partial class tipomaterialNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recursos_TipoMateriales_TipoMaterialId",
                table: "Recursos");

            migrationBuilder.AlterColumn<long>(
                name: "TipoMaterialId",
                table: "Recursos",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Recursos_TipoMateriales_TipoMaterialId",
                table: "Recursos",
                column: "TipoMaterialId",
                principalTable: "TipoMateriales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recursos_TipoMateriales_TipoMaterialId",
                table: "Recursos");

            migrationBuilder.AlterColumn<long>(
                name: "TipoMaterialId",
                table: "Recursos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recursos_TipoMateriales_TipoMaterialId",
                table: "Recursos",
                column: "TipoMaterialId",
                principalTable: "TipoMateriales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
