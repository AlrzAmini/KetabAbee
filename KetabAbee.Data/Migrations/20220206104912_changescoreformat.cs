using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KetabAbee.Data.Migrations
{
    public partial class changescoreformat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateScoreDate",
                table: "BookScores");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "BookScores",
                newName: "QualityScore");

            migrationBuilder.AddColumn<int>(
                name: "ContentScore",
                table: "BookScores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentScore",
                table: "BookScores");

            migrationBuilder.RenameColumn(
                name: "QualityScore",
                table: "BookScores",
                newName: "Score");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateScoreDate",
                table: "BookScores",
                type: "datetime2",
                nullable: true);
        }
    }
}
