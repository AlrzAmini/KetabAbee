using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changefktype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompareDetails_Compares_CompareId1",
                table: "CompareDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompareDetails_CompareId1",
                table: "CompareDetails");

            migrationBuilder.DropColumn(
                name: "CompareId1",
                table: "CompareDetails");

            migrationBuilder.AlterColumn<string>(
                name: "CompareId",
                table: "CompareDetails",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CompareDetails_CompareId",
                table: "CompareDetails",
                column: "CompareId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompareDetails_Compares_CompareId",
                table: "CompareDetails",
                column: "CompareId",
                principalTable: "Compares",
                principalColumn: "CompareId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompareDetails_Compares_CompareId",
                table: "CompareDetails");

            migrationBuilder.DropIndex(
                name: "IX_CompareDetails_CompareId",
                table: "CompareDetails");

            migrationBuilder.AlterColumn<int>(
                name: "CompareId",
                table: "CompareDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CompareId1",
                table: "CompareDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompareDetails_CompareId1",
                table: "CompareDetails",
                column: "CompareId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CompareDetails_Compares_CompareId1",
                table: "CompareDetails",
                column: "CompareId1",
                principalTable: "Compares",
                principalColumn: "CompareId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
