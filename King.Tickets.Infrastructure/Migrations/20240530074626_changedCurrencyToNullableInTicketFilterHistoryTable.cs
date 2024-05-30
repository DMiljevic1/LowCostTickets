using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace King.Tickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedCurrencyToNullableInTicketFilterHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "TicketFilterHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "TicketFilterHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
