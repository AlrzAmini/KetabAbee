using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addreltorequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AudioBookId",
                table: "AudioBookRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AudioBookRequests_AudioBookId",
                table: "AudioBookRequests",
                column: "AudioBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_AudioBookRequests_AudioBooks_AudioBookId",
                table: "AudioBookRequests",
                column: "AudioBookId",
                principalTable: "AudioBooks",
                principalColumn: "AudioBookId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AudioBookRequests_AudioBooks_AudioBookId",
                table: "AudioBookRequests");

            migrationBuilder.DropIndex(
                name: "IX_AudioBookRequests_AudioBookId",
                table: "AudioBookRequests");

            migrationBuilder.DropColumn(
                name: "AudioBookId",
                table: "AudioBookRequests");
        }
    }
}
