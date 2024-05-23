using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace King.Tickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTableNameToLowCostTicketsAndAddedCurrencyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "LowCostTickets");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "LowCostTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LowCostTickets",
                table: "LowCostTickets",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LowCostTickets",
                table: "LowCostTickets");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "LowCostTickets");

            migrationBuilder.RenameTable(
                name: "LowCostTickets",
                newName: "Tickets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");
        }
    }
}
