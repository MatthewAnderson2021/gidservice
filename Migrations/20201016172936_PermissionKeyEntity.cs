using Microsoft.EntityFrameworkCore.Migrations;

namespace GudelIdService.Migrations
{
    public partial class PermissionKeyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermissionKeys",
                columns: table => new
                {
                    Type = table.Column<int>(nullable: false),
                    GudelId_Id = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Hint = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionKeys", x => new { x.GudelId_Id, x.Type });
                    table.ForeignKey(
                        name: "FK_PermissionKeys_GudelId_GudelId_Id",
                        column: x => x.GudelId_Id,
                        principalTable: "GudelId",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionKeys");
        }
    }
}
