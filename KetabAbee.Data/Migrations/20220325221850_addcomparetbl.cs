using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addcomparetbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compares",
                columns: table => new
                {
                    CompareId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FirstBookId = table.Column<int>(type: "int", nullable: true),
                    SecondBookId = table.Column<int>(type: "int", nullable: true),
                    CompareDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compares", x => x.CompareId);
                    table.ForeignKey(
                        name: "FK_Compares_Books_FirstBookId",
                        column: x => x.FirstBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compares_Books_SecondBookId",
                        column: x => x.SecondBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compares_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compares_FirstBookId",
                table: "Compares",
                column: "FirstBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Compares_SecondBookId",
                table: "Compares",
                column: "SecondBookId");

            migrationBuilder.CreateIndex(
                name: "IX_Compares_UserId",
                table: "Compares",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Compares");
        }
    }
}
