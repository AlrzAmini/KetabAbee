using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addisansweredfrocontact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswered",
                table: "ContactUses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswered",
                table: "ContactUses");
        }
    }
}
