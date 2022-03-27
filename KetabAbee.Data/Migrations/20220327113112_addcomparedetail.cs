using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addcomparedetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompareDetails",
                columns: table => new
                {
                    CompareItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompareId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CompareId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompareDetails", x => x.CompareItemId);
                    table.ForeignKey(
                        name: "FK_CompareDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompareDetails_Compares_CompareId1",
                        column: x => x.CompareId1,
                        principalTable: "Compares",
                        principalColumn: "CompareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompareDetails_BookId",
                table: "CompareDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_CompareDetails_CompareId1",
                table: "CompareDetails",
                column: "CompareId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompareDetails");
        }
    }
}
