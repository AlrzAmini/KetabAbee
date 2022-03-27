using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changecompareprops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compares_Books_FirstBookId",
                table: "Compares");

            migrationBuilder.DropForeignKey(
                name: "FK_Compares_Books_SecondBookId",
                table: "Compares");

            migrationBuilder.DropIndex(
                name: "IX_Compares_FirstBookId",
                table: "Compares");

            migrationBuilder.DropIndex(
                name: "IX_Compares_SecondBookId",
                table: "Compares");

            migrationBuilder.DropColumn(
                name: "FirstBookId",
                table: "Compares");

            migrationBuilder.DropColumn(
                name: "SecondBookId",
                table: "Compares");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstBookId",
                table: "Compares",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondBookId",
                table: "Compares",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compares_FirstBookId",
                table: "Compares",
                column: "FirstBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Compares_SecondBookId",
                table: "Compares",
                column: "SecondBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Compares_Books_FirstBookId",
                table: "Compares",
                column: "FirstBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Compares_Books_SecondBookId",
                table: "Compares",
                column: "SecondBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
