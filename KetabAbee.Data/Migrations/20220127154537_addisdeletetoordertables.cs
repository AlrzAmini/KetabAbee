using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addisdeletetoordertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "OrderDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "OrderDetails");
        }
    }
}
