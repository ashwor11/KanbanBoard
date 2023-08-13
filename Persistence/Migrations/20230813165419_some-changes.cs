using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class somechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetails_Cards_CardId",
                table: "CardDetails");

            migrationBuilder.DropIndex(
                name: "IX_CardDetails_CardId",
                table: "CardDetails");

            migrationBuilder.AddColumn<string>(
                name: "JobDescription",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "CardDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "CardDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "CardDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_BoardId",
                table: "CardDetails",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDetails_Boards_BoardId",
                table: "CardDetails",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetails_Boards_BoardId",
                table: "CardDetails");

            migrationBuilder.DropIndex(
                name: "IX_CardDetails_BoardId",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "JobDescription",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "CardDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssignedDate",
                table: "CardDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_CardId",
                table: "CardDetails",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDetails_Cards_CardId",
                table: "CardDetails",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
