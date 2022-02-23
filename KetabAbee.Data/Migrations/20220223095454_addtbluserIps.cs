using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addtbluserIps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserIp",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "UserIps",
                columns: table => new
                {
                    UserIpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIps", x => x.UserIpId);
                    table.ForeignKey(
                        name: "FK_UserIps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserIps_UserId",
                table: "UserIps",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserIps");

            migrationBuilder.AddColumn<string>(
                name: "UserIp",
                table: "Users",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
