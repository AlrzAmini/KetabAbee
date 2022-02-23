using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class adduserIp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserIp",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIp",
                table: "Users");
        }
    }
}
