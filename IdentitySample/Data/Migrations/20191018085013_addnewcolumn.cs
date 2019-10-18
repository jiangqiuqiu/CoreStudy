using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentitySample.Data.Migrations
{
    public partial class addnewcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewCloumn",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewCloumn",
                table: "AspNetUsers");
        }
    }
}
