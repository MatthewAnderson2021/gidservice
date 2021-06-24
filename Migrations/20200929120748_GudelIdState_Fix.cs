using Microsoft.EntityFrameworkCore.Migrations;

namespace GudelIdService.Migrations
{
    public partial class GudelIdState_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("GudelIdState", "Id", "1", "Id", "0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("GudelIdState", "Id", "0", "Id", "1");
        }
    }
}
