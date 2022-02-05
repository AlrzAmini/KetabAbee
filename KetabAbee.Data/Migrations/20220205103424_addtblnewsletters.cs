using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addtblnewsletters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsLetters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsLetters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsEmailNewsLetter",
                columns: table => new
                {
                    NewsEmailsId = table.Column<int>(type: "int", nullable: false),
                    NewsLettersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsEmailNewsLetter", x => new { x.NewsEmailsId, x.NewsLettersId });
                    table.ForeignKey(
                        name: "FK_NewsEmailNewsLetter_NewsEmails_NewsEmailsId",
                        column: x => x.NewsEmailsId,
                        principalTable: "NewsEmails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsEmailNewsLetter_NewsLetters_NewsLettersId",
                        column: x => x.NewsLettersId,
                        principalTable: "NewsLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsEmailNewsLetter_NewsLettersId",
                table: "NewsEmailNewsLetter",
                column: "NewsLettersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsEmailNewsLetter");

            migrationBuilder.DropTable(
                name: "NewsLetters");
        }
    }
}
