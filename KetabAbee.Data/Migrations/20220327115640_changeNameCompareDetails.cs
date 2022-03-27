using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changeNameCompareDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompareDetails_Books_BookId",
                table: "CompareDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CompareDetails_Compares_CompareId",
                table: "CompareDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareDetails",
                table: "CompareDetails");

            migrationBuilder.RenameTable(
                name: "CompareDetails",
                newName: "CompareItems");

            migrationBuilder.RenameIndex(
                name: "IX_CompareDetails_CompareId",
                table: "CompareItems",
                newName: "IX_CompareItems_CompareId");

            migrationBuilder.RenameIndex(
                name: "IX_CompareDetails_BookId",
                table: "CompareItems",
                newName: "IX_CompareItems_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareItems",
                table: "CompareItems",
                column: "CompareItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompareItems_Books_BookId",
                table: "CompareItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompareItems_Compares_CompareId",
                table: "CompareItems",
                column: "CompareId",
                principalTable: "Compares",
                principalColumn: "CompareId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompareItems_Books_BookId",
                table: "CompareItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CompareItems_Compares_CompareId",
                table: "CompareItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompareItems",
                table: "CompareItems");

            migrationBuilder.RenameTable(
                name: "CompareItems",
                newName: "CompareDetails");

            migrationBuilder.RenameIndex(
                name: "IX_CompareItems_CompareId",
                table: "CompareDetails",
                newName: "IX_CompareDetails_CompareId");

            migrationBuilder.RenameIndex(
                name: "IX_CompareItems_BookId",
                table: "CompareDetails",
                newName: "IX_CompareDetails_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompareDetails",
                table: "CompareDetails",
                column: "CompareItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompareDetails_Books_BookId",
                table: "CompareDetails",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompareDetails_Compares_CompareId",
                table: "CompareDetails",
                column: "CompareId",
                principalTable: "Compares",
                principalColumn: "CompareId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
