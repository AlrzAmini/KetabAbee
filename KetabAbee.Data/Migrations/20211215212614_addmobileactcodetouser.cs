using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addmobileactcodetouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivationCode",
                table: "Users",
                newName: "EmailActivationCode");

            migrationBuilder.AddColumn<string>(
                name: "MobileActivationCode",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileActivationCode",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "EmailActivationCode",
                table: "Users",
                newName: "ActivationCode");
        }
    }
}
