using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace King.Tickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedFilterHistoryTableAndAddedForeignKeyWithLowCostTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FilterHistoryId",
                table: "LowCostTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfStops",
                table: "LowCostTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "LowCostTickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FilterHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivalAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureAirport = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumberOfPassengers = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LowCostTickets_FilterHistoryId",
                table: "LowCostTickets",
                column: "FilterHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LowCostTickets_FilterHistory_FilterHistoryId",
                table: "LowCostTickets",
                column: "FilterHistoryId",
                principalTable: "FilterHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LowCostTickets_FilterHistory_FilterHistoryId",
                table: "LowCostTickets");

            migrationBuilder.DropTable(
                name: "FilterHistory");

            migrationBuilder.DropIndex(
                name: "IX_LowCostTickets_FilterHistoryId",
                table: "LowCostTickets");

            migrationBuilder.DropColumn(
                name: "FilterHistoryId",
                table: "LowCostTickets");

            migrationBuilder.DropColumn(
                name: "NumberOfStops",
                table: "LowCostTickets");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "LowCostTickets");
        }
    }
}
