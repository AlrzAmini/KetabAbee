using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class adddownlodedaudios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DownloadedAudioBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AudioBookId = table.Column<int>(type: "int", nullable: false),
                    UserIp = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    DownloadDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadedAudioBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DownloadedAudioBooks_AudioBooks_AudioBookId",
                        column: x => x.AudioBookId,
                        principalTable: "AudioBooks",
                        principalColumn: "AudioBookId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DownloadedAudioBooks_AudioBookId",
                table: "DownloadedAudioBooks",
                column: "AudioBookId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DownloadedAudioBooks");
        }
    }
}
