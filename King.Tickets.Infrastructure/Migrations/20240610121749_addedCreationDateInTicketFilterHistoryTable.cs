using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace King.Tickets.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCreationDateInTicketFilterHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "TicketFilterHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "TicketFilterHistory");
        }
    }
}
