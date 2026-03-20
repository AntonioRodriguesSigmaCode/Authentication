using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.Migrations
{
    /// <inheritdoc />
    public partial class xixa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<int>(
            //    name: "RoleId",
            //    table: "Permissoes",
            //    type: "int",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "RoleUtilizador",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UtilizadoresId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUtilizador", x => new { x.RolesId, x.UtilizadoresId });
                    table.ForeignKey(
                        name: "FK_RoleUtilizador_AspNetUsers_UtilizadoresId",
                        column: x => x.UtilizadoresId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUtilizador_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Permissoes_RoleId",
            //    table: "Permissoes",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RoleUtilizador_UtilizadoresId",
            //    table: "RoleUtilizador",
            //    column: "UtilizadoresId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Permissoes_Roles_RoleId",
            //    table: "Permissoes",
            //    column: "RoleId",
            //    principalTable: "Roles",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permissoes_Roles_RoleId",
                table: "Permissoes");

            migrationBuilder.DropTable(
                name: "RoleUtilizador");

            migrationBuilder.DropIndex(
                name: "IX_Permissoes_RoleId",
                table: "Permissoes");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Permissoes");
        }
    }
}
