using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class addqtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ABookQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    AudioBookId = table.Column<int>(type: "int", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ABookQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ABookQuestions_AudioBooks_AudioBookId",
                        column: x => x.AudioBookId,
                        principalTable: "AudioBooks",
                        principalColumn: "AudioBookId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ABookQuestions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ABookQAnswers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ABookQAnswers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_ABookQAnswers_ABookQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ABookQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ABookQAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ABookQAnswers_QuestionId",
                table: "ABookQAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ABookQAnswers_UserId",
                table: "ABookQAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ABookQuestions_AudioBookId",
                table: "ABookQuestions",
                column: "AudioBookId");

            migrationBuilder.CreateIndex(
                name: "IX_ABookQuestions_UserId",
                table: "ABookQuestions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ABookQAnswers");

            migrationBuilder.DropTable(
                name: "ABookQuestions");
        }
    }
}
