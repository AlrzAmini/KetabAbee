using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changebook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubGroup2Id",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubGroupId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_SubGroup2Id",
                table: "Books",
                column: "SubGroup2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SubGroupId",
                table: "Books",
                column: "SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ProductGroups_SubGroup2Id",
                table: "Books",
                column: "SubGroup2Id",
                principalTable: "ProductGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ProductGroups_SubGroupId",
                table: "Books",
                column: "SubGroupId",
                principalTable: "ProductGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ProductGroups_SubGroup2Id",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_ProductGroups_SubGroupId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SubGroup2Id",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_SubGroupId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SubGroup2Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "SubGroupId",
                table: "Books");
        }
    }
}
