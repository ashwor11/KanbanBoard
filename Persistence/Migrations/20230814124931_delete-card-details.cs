using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deletecarddetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedPersonId = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReassigned = table.Column<bool>(type: "bit", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardDetails_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardDetails_Persons_AssignedPersonId",
                        column: x => x.AssignedPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_AssignedPersonId",
                table: "CardDetails",
                column: "AssignedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_BoardId",
                table: "CardDetails",
                column: "BoardId");
        }
    }
}
