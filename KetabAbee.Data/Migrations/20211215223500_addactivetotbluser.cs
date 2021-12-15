using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addactivetotbluser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Users",
                newName: "IsMobileActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailActive",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsMobileActive",
                table: "Users",
                newName: "IsActive");
        }
    }
}
